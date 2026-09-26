namespace CopilotTokenTracker
{
    public partial class Form1 : Form
    {
        private const string UserVariable = "CopilotTokenTracker_User";
        private const string PatVariable = "CopilotTokenTracker_PAT";
        private const string ThemeVariable = "CopilotTokenTracker_Theme";
        private const string MaskedPat = "**********";
        private const int AuthPanelHeight = 160;
        private const int SlideStep = 12;
        private static readonly TimeSpan ApiDelay = TimeSpan.FromSeconds(1);

        private CancellationTokenSource? _cts;
        private string _pat = string.Empty;
        private bool _authOpen;
        private DateTimeOffset? _lastSampleTime;
        private CopilotUsage? _lastSample;
        private string _firstSampleLabel = "App Launch";
        private CopilotUsage? _sessionStartSample;

        public Form1()
        {
            InitializeComponent();
            LoadThemePreference();
            ApplyTheme();
            LoadCredentials();
            Shown += Form1_Shown;
            Microsoft.Win32.SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
        }

        private void ApplyTheme()
        {
            Theme.Apply(this);
            UpdateAuthStatus();
        }

        private void SystemEvents_UserPreferenceChanged(object sender, Microsoft.Win32.UserPreferenceChangedEventArgs e)
        {
            if (Theme.Mode == ThemeMode.System &&
                e.Category is Microsoft.Win32.UserPreferenceCategory.Color or Microsoft.Win32.UserPreferenceCategory.VisualStyle)
            {
                BeginInvoke(ApplyTheme);
            }
        }

        private void LoadThemePreference()
        {
            var saved = UserEnvironment.Get(ThemeVariable);

            if (!Enum.TryParse(saved, ignoreCase: true, out ThemeMode mode))
            {
                mode = ThemeMode.System;
            }

            Theme.Mode = mode;
            comboTheme.SelectedItem = mode.ToString();
        }

        private void comboTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Enum.TryParse(comboTheme.SelectedItem as string, ignoreCase: true, out ThemeMode mode) || mode == Theme.Mode)
            {
                return;
            }

            Theme.Mode = mode;
            ApplyTheme();

            try
            {
                UserEnvironment.Set(ThemeVariable, mode.ToString());
            }
            catch (Exception ex)
            {
                Log($"{DateTimeOffset.Now:HH:mm:ss}  could not save theme preference: {ex.Message}");
            }
        }

        private void LoadCredentials()
        {
            textBoxUser.Text = UserEnvironment.Get(UserVariable) ?? string.Empty;
            _pat = UserEnvironment.Get(PatVariable) ?? string.Empty;

            if (_pat.Length > 0)
            {
                textBoxPat.UseSystemPasswordChar = false;
                textBoxPat.Text = MaskedPat;
            }

            UpdateAuthStatus();

            if (string.IsNullOrWhiteSpace(textBoxUser.Text) || _pat.Length == 0)
            {
                ToggleAuthPanel(true);
            }
        }

        private async void Form1_Shown(object? sender, EventArgs e)
        {
            await RefreshUsageAsync();
        }

        private async Task RefreshUsageAsync()
        {
            var user = textBoxUser.Text.Trim();

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(_pat) || _cts is not null)
            {
                return;
            }

            using var client = new GitHubUsageClient(_pat);

            try
            {
                SetBusy(true);
                await Task.Delay(ApiDelay);
                var usage = await client.GetCopilotUsageAsync(user, CancellationToken.None);
                ShowUsage(usage);
            }
            catch (Exception ex)
            {
                Log($"{DateTimeOffset.Now:HH:mm:ss}  initial fetch failed: {ex.Message}");
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void SetBusy(bool busy)
        {
            progressSpinner.Spinning = busy;

            if (busy)
            {
                progressSpinner.BringToFront();
            }
        }

        private void ShowUsage(CopilotUsage usage)
        {
            labelTokens.Text = $"Total Tokens Spent: {usage.Quantity:N0} / {usage.IncludedRequests:N0} included";
            labelDollars.Text = $"Total Dollars Spent: {usage.GrossAmount:C2}";

            var now = DateTimeOffset.Now;

            if (_sessionStartSample is { } baseline)
            {
                // Ongoing poll: one line with the current time and the spend since the session started.
                var tokens = usage.Quantity - baseline.Quantity;
                var dollars = usage.GrossAmount - baseline.GrossAmount;
                Log($"    {FormatTime(now)} - {tokens:N2} tokens ({dollars:C2})");
            }
            else
            {
                Log(_firstSampleLabel);
                Log($"    {FormatTime(now)}");
                Log($"    {usage.Quantity:N2} tokens ({usage.GrossAmount:C2})");
            }

            _lastSampleTime = now;
            _lastSample = usage;
        }

        private string GetTaskName()
        {
            var taskName = textBoxTaskName.Text.Trim();
            return string.IsNullOrEmpty(taskName) ? "(no task name)" : taskName;
        }

        private static string FormatTime(DateTimeOffset value) => value.ToString("yyyy/MM/dd h:mm:ss tt");

        private void UpdateAuthStatus()
        {
            var signedIn = !string.IsNullOrWhiteSpace(textBoxUser.Text) && _pat.Length > 0;
            buttonAuth.ForeColor = signedIn ? Theme.SignedInGlyph : Theme.SignedOutGlyph;
        }

        private void buttonAuth_Click(object sender, EventArgs e) => ToggleAuthPanel(!_authOpen);

        private void ToggleAuthPanel(bool open)
        {
            _authOpen = open;
            timerSlide.Start();
        }

        private void timerSlide_Tick(object sender, EventArgs e)
        {
            var target = _authOpen ? AuthPanelHeight : 0;
            var height = panelAuth.Height;

            if (height == target)
            {
                timerSlide.Stop();
                return;
            }

            height += height < target ? SlideStep : -SlideStep;
            height = _authOpen ? Math.Min(height, target) : Math.Max(height, target);

            var delta = height - panelAuth.Height;
            panelAuth.Height = height;

            // Grow (or shrink) the form by the same amount so the main panel keeps its size.
            if (WindowState == FormWindowState.Normal)
            {
                Height += delta;
            }

            panelMain.Top = panelAuth.Top + panelAuth.Height + (panelAuth.Height > 0 ? 10 : 0);
            panelMain.Height = ClientSize.Height - panelMain.Top - 12;
        }

        private void textBoxPat_Enter(object sender, EventArgs e)
        {
            if (textBoxPat.Text == MaskedPat)
            {
                textBoxPat.UseSystemPasswordChar = true;
                textBoxPat.Clear();
            }
        }

        private async void buttonSave_Click(object sender, EventArgs e)
        {
            var user = textBoxUser.Text.Trim();
            var pat = textBoxPat.Text == MaskedPat ? _pat : textBoxPat.Text;

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pat))
            {
                MessageBox.Show(this, "Username and PAT are required.", "Copilot Token Tracker",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                UserEnvironment.Set(UserVariable, user);
                UserEnvironment.Set(PatVariable, pat);
            }
            catch (Exception ex)
            {
                Log($"{DateTimeOffset.Now:HH:mm:ss}  could not save credentials: {ex.Message}");
                return;
            }

            _pat = pat;
            _firstSampleLabel = "Initialize";
            textBoxPat.UseSystemPasswordChar = false;
            textBoxPat.Text = MaskedPat;

            UpdateAuthStatus();
            Log($"{DateTimeOffset.Now:HH:mm:ss}  credentials saved to user environment variables.");
            ToggleAuthPanel(false);

            await RefreshUsageAsync();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Remove the saved username and PAT?", "Copilot Token Tracker",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                UserEnvironment.Set(UserVariable, string.Empty);
                UserEnvironment.Set(PatVariable, string.Empty);
            }
            catch (Exception ex)
            {
                Log($"{DateTimeOffset.Now:HH:mm:ss}  could not clear credentials: {ex.Message}");
                return;
            }

            _pat = string.Empty;
            _lastSample = null;
            _lastSampleTime = null;
            _firstSampleLabel = "Initialize";

            textBoxUser.Clear();
            textBoxPat.UseSystemPasswordChar = true;
            textBoxPat.Clear();

            labelTokens.Text = "Total Tokens Spent: -";
            labelDollars.Text = "Total Dollars Spent: -";

            UpdateAuthStatus();
            Log($"{DateTimeOffset.Now:HH:mm:ss}  credentials cleared.");
        }

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            if (_cts is not null)
            {
                _cts.Cancel();
                return;
            }

            var user = textBoxUser.Text.Trim();

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(_pat))
            {
                MessageBox.Show(this, "Enter and save your credentials first.", "Copilot Token Tracker",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ToggleAuthPanel(true);
                return;
            }

            _cts = new CancellationTokenSource();
            SetRunningState(true);

            try
            {
                await PollAsync(user, _pat, TimeSpan.FromMinutes((double)numericInterval.Value), _cts.Token);
            }
            finally
            {
                _cts.Dispose();
                _cts = null;
                SetRunningState(false);
            }
        }

        private async Task PollAsync(string user, string pat, TimeSpan interval, CancellationToken token)
        {
            using var client = new GitHubUsageClient(pat);
            using var timer = new PeriodicTimer(interval);

            // Header for the whole polling session; samples below are indented under it.
            Log(GetTaskName());
            Log($"    {FormatTime(DateTimeOffset.Now)}");
            _sessionStartSample = _lastSample;

            do
            {
                try
                {
                    SetBusy(true);
                    await Task.Delay(ApiDelay, token);
                    var usage = await client.GetCopilotUsageAsync(user, token);

                    _sessionStartSample ??= usage;
                    ShowUsage(usage);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Log($"{DateTimeOffset.Now:HH:mm:ss}  poll failed: {ex.Message}");
                }
                finally
                {
                    SetBusy(false);
                }
            }
            while (await SafeWaitAsync(timer, token));

            var baseline = _sessionStartSample;
            _sessionStartSample = null;

            // The end time and session totals are only meaningful once polling has actually stopped.
            Log($"    {FormatTime(DateTimeOffset.Now)}");

            if (baseline is { } start && _lastSample is { } final)
            {
                Log($"    Total: {final.Quantity - start.Quantity:N2} tokens ({final.GrossAmount - start.GrossAmount:C2})");
            }

            Log($"{DateTimeOffset.Now:HH:mm:ss}  polling stopped.");
        }

        private static async Task<bool> SafeWaitAsync(PeriodicTimer timer, CancellationToken token)
        {
            try { return await timer.WaitForNextTickAsync(token); }
            catch (OperationCanceledException) { return false; }
        }

        private void SetRunningState(bool running)
        {
            buttonStart.Text = running ? "Stop" : "Start";
            buttonAuth.Enabled = !running;
            numericInterval.Enabled = !running;
        }

        private void Log(string message)
        {
            listBoxLog.Items.Add(message);
            listBoxLog.TopIndex = listBoxLog.Items.Count - 1;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _cts?.Cancel();
            Microsoft.Win32.SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;
            base.OnFormClosing(e);
        }

        private void labelDollars_Click(object sender, EventArgs e)
        {

        }
    }
}
