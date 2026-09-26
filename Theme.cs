using Microsoft.Win32;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace CopilotTokenTracker
{
    /// <summary>
    ///  Theme selection made by the user.
    /// </summary>
    internal enum ThemeMode
    {
        System,
        Light,
        Dark
    }

    /// <summary>
    ///  Light/dark palette resolved from the current Windows personalization setting.
    /// </summary>
    internal static class Theme
    {
        private const string PersonalizeKey = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

        public static ThemeMode Mode { get; set; } = ThemeMode.System;

        public static bool IsDark => Mode switch
        {
            ThemeMode.Light => false,
            ThemeMode.Dark => true,
            _ => !IsSystemLightTheme()
        };

        public static Color Background => IsDark ? Color.FromArgb(32, 32, 32) : SystemColors.Control;

        public static Color Foreground => IsDark ? Color.FromArgb(240, 240, 240) : SystemColors.ControlText;

        public static Color InputBackground => IsDark ? Color.FromArgb(45, 45, 45) : SystemColors.Window;

        public static Color InputForeground => IsDark ? Color.FromArgb(240, 240, 240) : SystemColors.WindowText;

        public static Color ButtonBackground => IsDark ? Color.FromArgb(56, 56, 56) : SystemColors.Control;

        public static Color HoverBackground => IsDark ? Color.FromArgb(62, 62, 62) : Color.FromArgb(229, 229, 229);

        public static Color PressedBackground => IsDark ? Color.FromArgb(78, 78, 78) : Color.FromArgb(204, 204, 204);

        /// <summary>
        ///  Title bar fill. Explicitly set so Windows does not paint the user's accent
        ///  colour on the active caption.
        /// </summary>
        public static Color CaptionBackground => IsDark ? Color.FromArgb(32, 32, 32) : Color.FromArgb(255, 255, 255);

        public static Color CaptionForeground => IsDark ? Color.FromArgb(240, 240, 240) : Color.FromArgb(26, 26, 26);

        /// <summary>
        ///  Colour of the account glyph when no credentials are stored.
        ///  Pale in dark mode, gray in light mode.
        /// </summary>
        public static Color SignedOutGlyph => IsDark ? Color.FromArgb(214, 214, 214) : Color.Gray;

        public static Color SignedInGlyph => IsDark ? Color.FromArgb(106, 200, 106) : Color.ForestGreen;

        private static bool IsSystemLightTheme()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(PersonalizeKey);
                return key?.GetValue("AppsUseLightTheme") is not int value || value != 0;
            }
            catch
            {
                return true;
            }
        }

        public static void Apply(Control root)
        {
            root.BackColor = Background;
            root.ForeColor = Foreground;

            if (root is Form form)
            {
                ApplyTitleBar(form);
            }

            ApplyToChildren(root);
        }

        // DWMWA_USE_IMMERSIVE_DARK_MODE. Attribute 20 on Windows 10 2004+ and Windows 11;
        // earlier Windows 10 builds (1809-1909) used the pre-release value 19.
        private const int DwmUseImmersiveDarkMode = 20;
        private const int DwmUseImmersiveDarkModeLegacy = 19;

        // DWMWA_CAPTION_COLOR / DWMWA_TEXT_COLOR, Windows 11 22000+.
        private const int DwmCaptionColor = 35;
        private const int DwmTextColor = 36;

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);

        // DWM expects a COLORREF (0x00BBGGRR), which is byte-reversed from Color.ToArgb.
        private static int ToColorRef(Color color) => color.R | (color.G << 8) | (color.B << 16);

        /// <summary>
        ///  Repaints the non-client area (title bar and border) to match the theme.
        /// </summary>
        private static void ApplyTitleBar(Form form)
        {
            if (!form.IsHandleCreated)
            {
                // The attribute cannot be set before there is a window; retry once the handle exists.
                form.HandleCreated -= TitleBarOnHandleCreated;
                form.HandleCreated += TitleBarOnHandleCreated;
                return;
            }

            var dark = IsDark ? 1 : 0;

            if (DwmSetWindowAttribute(form.Handle, DwmUseImmersiveDarkMode, ref dark, sizeof(int)) != 0)
            {
                DwmSetWindowAttribute(form.Handle, DwmUseImmersiveDarkModeLegacy, ref dark, sizeof(int));
            }

            // Without this the active caption uses the personalization accent colour.
            var caption = ToColorRef(CaptionBackground);
            DwmSetWindowAttribute(form.Handle, DwmCaptionColor, ref caption, sizeof(int));

            var text = ToColorRef(CaptionForeground);
            DwmSetWindowAttribute(form.Handle, DwmTextColor, ref text, sizeof(int));

            // DWM only repaints the frame on the next non-client change, so nudge it.
            if (form.Visible && form.WindowState == FormWindowState.Normal)
            {
                form.Width += 1;
                form.Width -= 1;
            }
        }

        private static void TitleBarOnHandleCreated(object? sender, EventArgs e)
        {
            if (sender is Form form)
            {
                form.HandleCreated -= TitleBarOnHandleCreated;
                ApplyTitleBar(form);
            }
        }

        private static void ApplyToChildren(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                switch (control)
                {
                    case TextBox textBox:
                        textBox.BackColor = InputBackground;
                        textBox.ForeColor = InputForeground;
                        textBox.BorderStyle = BorderStyle.FixedSingle;
                        break;
                    case ListBox listBox:
                        listBox.BackColor = InputBackground;
                        listBox.ForeColor = InputForeground;
                        listBox.BorderStyle = BorderStyle.FixedSingle;
                        break;
                    case NumericUpDown numeric:
                        numeric.BackColor = InputBackground;
                        numeric.ForeColor = InputForeground;
                        break;
                    case Button button:
                        if (button.FlatStyle == FlatStyle.Flat)
                        {
                            // Borderless glyph buttons should blend into the form, not sit on a raised face.
                            button.UseVisualStyleBackColor = false;
                            button.BackColor = Background;
                            button.FlatAppearance.BorderSize = 0;
                            button.FlatAppearance.MouseOverBackColor = HoverBackground;
                            button.FlatAppearance.MouseDownBackColor = PressedBackground;
                        }
                        else
                        {
                            button.UseVisualStyleBackColor = !IsDark;
                            button.BackColor = ButtonBackground;
                            button.ForeColor = Foreground;
                        }
                        break;
                    case ComboBox combo:
                        combo.BackColor = InputBackground;
                        combo.ForeColor = InputForeground;
                        combo.FlatStyle = IsDark ? FlatStyle.Flat : FlatStyle.Standard;
                        break;
                    case CircularSpinner spinner:
                        spinner.BackColor = Background;
                        spinner.ForeColor = SignedInGlyph;
                        break;
                    default:
                        control.BackColor = Background;
                        control.ForeColor = Foreground;
                        break;
                }

                if (control.HasChildren)
                {
                    ApplyToChildren(control);
                }
            }
        }
    }
}
