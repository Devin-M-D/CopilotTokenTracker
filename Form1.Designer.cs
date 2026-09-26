namespace CopilotTokenTracker
{
	partial class Form1
	{
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			buttonAuth = new Button();
			panelAuth = new Panel();
			labelUser = new Label();
			textBoxUser = new TextBox();
			labelPat = new Label();
			textBoxPat = new TextBox();
			buttonSave = new Button();
			buttonClear = new Button();
			labelTheme = new Label();
			comboTheme = new ComboBox();
			panelMain = new Panel();
			labelTokens = new Label();
			labelDollars = new Label();
			listBoxLog = new ListBox();
			labelInterval = new Label();
			numericInterval = new NumericUpDown();
			labelTaskName = new Label();
			textBoxTaskName = new TextBox();
			buttonStart = new Button();
			progressSpinner = new CircularSpinner();
			timerSlide = new System.Windows.Forms.Timer(components);
			panelAuth.SuspendLayout();
			panelMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)numericInterval).BeginInit();
			SuspendLayout();
			// 
			// buttonAuth
			// 
			buttonAuth.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			buttonAuth.FlatAppearance.BorderSize = 0;
			buttonAuth.FlatStyle = FlatStyle.Flat;
			buttonAuth.Font = new Font("Segoe UI Emoji", 14F);
			buttonAuth.ForeColor = Color.Gray;
			buttonAuth.Location = new Point(430, 8);
			buttonAuth.Name = "buttonAuth";
			buttonAuth.Size = new Size(40, 40);
			buttonAuth.TabIndex = 3;
			buttonAuth.Text = "👤";
			buttonAuth.UseVisualStyleBackColor = true;
			buttonAuth.Click += buttonAuth_Click;
			// 
			// panelAuth
			// 
			panelAuth.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			panelAuth.Controls.Add(labelUser);
			panelAuth.Controls.Add(textBoxUser);
			panelAuth.Controls.Add(labelPat);
			panelAuth.Controls.Add(textBoxPat);
			panelAuth.Controls.Add(buttonSave);
			panelAuth.Controls.Add(buttonClear);
			panelAuth.Controls.Add(labelTheme);
			panelAuth.Controls.Add(comboTheme);
			panelAuth.Location = new Point(12, 94);
			panelAuth.Name = "panelAuth";
			panelAuth.Size = new Size(458, 0);
			panelAuth.TabIndex = 6;
			// 
			// labelUser
			// 
			labelUser.AutoSize = true;
			labelUser.Location = new Point(3, 11);
			labelUser.Name = "labelUser";
			labelUser.Size = new Size(78, 20);
			labelUser.TabIndex = 0;
			labelUser.Text = "Username:";
			// 
			// textBoxUser
			// 
			textBoxUser.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			textBoxUser.Location = new Point(110, 8);
			textBoxUser.Name = "textBoxUser";
			textBoxUser.Size = new Size(345, 27);
			textBoxUser.TabIndex = 1;
			// 
			// labelPat
			// 
			labelPat.AutoSize = true;
			labelPat.Location = new Point(3, 46);
			labelPat.Name = "labelPat";
			labelPat.Size = new Size(87, 20);
			labelPat.TabIndex = 2;
			labelPat.Text = "GitHub PAT:";
			// 
			// textBoxPat
			// 
			textBoxPat.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			textBoxPat.Location = new Point(110, 43);
			textBoxPat.Name = "textBoxPat";
			textBoxPat.Size = new Size(345, 27);
			textBoxPat.TabIndex = 3;
			textBoxPat.UseSystemPasswordChar = true;
			textBoxPat.Enter += textBoxPat_Enter;
			// 
			// buttonSave
			// 
			buttonSave.Location = new Point(110, 80);
			buttonSave.Name = "buttonSave";
			buttonSave.Size = new Size(150, 30);
			buttonSave.TabIndex = 4;
			buttonSave.Text = "Save credentials";
			buttonSave.UseVisualStyleBackColor = true;
			buttonSave.Click += buttonSave_Click;
			// 
			// buttonClear
			// 
			buttonClear.Location = new Point(270, 80);
			buttonClear.Name = "buttonClear";
			buttonClear.Size = new Size(150, 30);
			buttonClear.TabIndex = 5;
			buttonClear.Text = "Clear credentials";
			buttonClear.UseVisualStyleBackColor = true;
			buttonClear.Click += buttonClear_Click;
			// 
			// labelTheme
			// 
			labelTheme.AutoSize = true;
			labelTheme.Location = new Point(3, 123);
			labelTheme.Name = "labelTheme";
			labelTheme.Size = new Size(55, 20);
			labelTheme.TabIndex = 6;
			labelTheme.Text = "Theme:";
			// 
			// comboTheme
			// 
			comboTheme.DropDownStyle = ComboBoxStyle.DropDownList;
			comboTheme.Items.AddRange(new object[] { "System", "Light", "Dark" });
			comboTheme.Location = new Point(110, 119);
			comboTheme.Name = "comboTheme";
			comboTheme.Size = new Size(150, 28);
			comboTheme.TabIndex = 7;
			comboTheme.SelectedIndexChanged += comboTheme_SelectedIndexChanged;
			// 
			// panelMain
			// 
			panelMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			panelMain.Controls.Add(labelTokens);
			panelMain.Controls.Add(labelDollars);
			panelMain.Controls.Add(listBoxLog);
			panelMain.Location = new Point(12, 94);
			panelMain.Name = "panelMain";
			panelMain.Size = new Size(458, 262);
			panelMain.TabIndex = 4;
			// 
			// labelTokens
			// 
			labelTokens.AutoSize = true;
			labelTokens.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
			labelTokens.Location = new Point(3, 8);
			labelTokens.Name = "labelTokens";
			labelTokens.Size = new Size(318, 28);
			labelTokens.TabIndex = 0;
			labelTokens.Text = "Total Tokens Spent: 0000 / 0000";
			// 
			// labelDollars
			// 
			labelDollars.AutoSize = true;
			labelDollars.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
			labelDollars.Location = new Point(3, 36);
			labelDollars.Name = "labelDollars";
			labelDollars.Size = new Size(211, 28);
			labelDollars.TabIndex = 1;
			labelDollars.Text = "Total Dollars Spent: -";
			labelDollars.Click += labelDollars_Click;
			// 
			// listBoxLog
			// 
			listBoxLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			listBoxLog.Font = new Font("Consolas", 9F);
			listBoxLog.FormattingEnabled = true;
			listBoxLog.IntegralHeight = false;
			listBoxLog.Location = new Point(3, 80);
			listBoxLog.Name = "listBoxLog";
			listBoxLog.Size = new Size(452, 179);
			listBoxLog.TabIndex = 2;
			// 
			// labelInterval
			// 
			labelInterval.AutoSize = true;
			labelInterval.Location = new Point(12, 19);
			labelInterval.Name = "labelInterval";
			labelInterval.Size = new Size(100, 20);
			labelInterval.TabIndex = 0;
			labelInterval.Text = "Interval (min):";
			// 
			// numericInterval
			// 
			numericInterval.Location = new Point(118, 15);
			numericInterval.Maximum = new decimal(new int[] { 1440, 0, 0, 0 });
			numericInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			numericInterval.Name = "numericInterval";
			numericInterval.Size = new Size(80, 27);
			numericInterval.TabIndex = 1;
			numericInterval.Value = new decimal(new int[] { 5, 0, 0, 0 });
			// 
			// labelTaskName
			// 
			labelTaskName.AutoSize = true;
			labelTaskName.Location = new Point(12, 59);
			labelTaskName.Name = "labelTaskName";
			labelTaskName.Size = new Size(80, 20);
			labelTaskName.TabIndex = 6;
			labelTaskName.Text = "Task name:";
			// 
			// textBoxTaskName
			// 
			textBoxTaskName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			textBoxTaskName.Location = new Point(118, 55);
			textBoxTaskName.Name = "textBoxTaskName";
			textBoxTaskName.Size = new Size(352, 27);
			textBoxTaskName.TabIndex = 7;
			// 
			// buttonStart
			// 
			buttonStart.Location = new Point(214, 14);
			buttonStart.Name = "buttonStart";
			buttonStart.Size = new Size(90, 29);
			buttonStart.TabIndex = 2;
			buttonStart.Text = "Start";
			buttonStart.UseVisualStyleBackColor = true;
			buttonStart.Click += buttonStart_Click;
			// 
			// progressSpinner
			// 
			progressSpinner.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			progressSpinner.ForeColor = Color.ForestGreen;
			progressSpinner.Location = new Point(388, 14);
			progressSpinner.Name = "progressSpinner";
			progressSpinner.Size = new Size(28, 28);
			progressSpinner.TabIndex = 8;
			progressSpinner.TabStop = false;
			progressSpinner.Visible = false;
			// 
			// timerSlide
			// 
			timerSlide.Interval = 15;
			timerSlide.Tick += timerSlide_Tick;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(482, 368);
			Controls.Add(progressSpinner);
			Controls.Add(panelMain);
			Controls.Add(panelAuth);
			Controls.Add(buttonAuth);
			Controls.Add(buttonStart);
			Controls.Add(numericInterval);
			Controls.Add(labelInterval);
			Controls.Add(textBoxTaskName);
			Controls.Add(labelTaskName);
			MinimumSize = new Size(420, 360);
			Name = "Form1";
			Text = "Copilot Token Tracker";
			panelAuth.ResumeLayout(false);
			panelAuth.PerformLayout();
			panelMain.ResumeLayout(false);
			panelMain.PerformLayout();
			((System.ComponentModel.ISupportInitialize)numericInterval).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		private System.Windows.Forms.Button buttonAuth;
		private System.Windows.Forms.Panel panelAuth;
		private System.Windows.Forms.Label labelUser;
		private System.Windows.Forms.TextBox textBoxUser;
		private System.Windows.Forms.Label labelPat;
		private System.Windows.Forms.TextBox textBoxPat;
		private System.Windows.Forms.Button buttonSave;
		private System.Windows.Forms.Button buttonClear;
		private System.Windows.Forms.Label labelTheme;
		private System.Windows.Forms.ComboBox comboTheme;
		private System.Windows.Forms.Panel panelMain;
		private System.Windows.Forms.Label labelInterval;
		private System.Windows.Forms.NumericUpDown numericInterval;
		private System.Windows.Forms.Label labelTaskName;
		private System.Windows.Forms.TextBox textBoxTaskName;
		private System.Windows.Forms.Button buttonStart;
		private CircularSpinner progressSpinner;
		private System.Windows.Forms.Label labelTokens;
		private System.Windows.Forms.Label labelDollars;
		private System.Windows.Forms.ListBox listBoxLog;
		private System.Windows.Forms.Timer timerSlide;

        #endregion
    }
}
