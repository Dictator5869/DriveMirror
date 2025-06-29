// Form1.Designer.cs
namespace DriveMirror
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //
            // Main Tab Start
            //
            TabControl tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            TabPage mainTab = new TabPage("ManualSync");
            //
            //Instantiate controls and suspend layout
            //
            components = new System.ComponentModel.Container();
            comboSource = new ComboBox();
            comboDest = new ComboBox();
            btnSyncNow = new Button();
            btnRefreshDrives = new Button();
            txtLog = new TextBox();
            notifyIcon1 = new NotifyIcon(components);
            SuspendLayout();
            // 
            // comboSource
            // 
            comboSource.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSource.FormattingEnabled = true;
            comboSource.Location = new Point(16, 18);
            comboSource.Margin = new Padding(4, 5, 4, 5);
            comboSource.Name = "comboSource";
            comboSource.Size = new Size(160, 28);
            comboSource.TabIndex = 0;
            // 
            // comboDest
            // 
            comboDest.DropDownStyle = ComboBoxStyle.DropDownList;
            comboDest.FormattingEnabled = true;
            comboDest.Location = new Point(200, 18);
            comboDest.Margin = new Padding(4, 5, 4, 5);
            comboDest.Name = "comboDest";
            comboDest.Size = new Size(160, 28);
            comboDest.TabIndex = 1;
            // 
            // btnSyncNow
            // 
            btnSyncNow.Location = new Point(383, 18);
            btnSyncNow.Margin = new Padding(4, 5, 4, 5);
            btnSyncNow.Name = "btnSyncNow";
            btnSyncNow.Size = new Size(100, 35);
            btnSyncNow.TabIndex = 2;
            btnSyncNow.Text = "Sync Now";
            btnSyncNow.UseVisualStyleBackColor = true;
            btnSyncNow.Click += btnSyncNow_Click;
            // 
            // btnRefreshDrives
            // 
            btnRefreshDrives.Location = new Point(493, 18);
            btnRefreshDrives.Margin = new Padding(4, 5, 4, 5);
            btnRefreshDrives.Name = "btnRefreshDrives";
            btnRefreshDrives.Size = new Size(120, 35);
            btnRefreshDrives.TabIndex = 4;
            btnRefreshDrives.Text = "Refresh";
            btnRefreshDrives.UseVisualStyleBackColor = true;
            btnRefreshDrives.Click += btnRefreshDrives_Click;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(18, 63);
            txtLog.Margin = new Padding(4, 5, 4, 5);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(595, 306);
            txtLog.TabIndex = 3;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(624, 403);
            mainTab.Controls.Add(txtLog);
            mainTab.Controls.Add(btnRefreshDrives);
            mainTab.Controls.Add(btnSyncNow);
            mainTab.Controls.Add(comboDest);
            mainTab.Controls.Add(comboSource);
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "Drive Mirror";
            Resize += Form1_Resize;
            //
            //Main Tab End
            //
            tabControl.TabPages.Add(mainTab);
            //
            //Scheduling Tab
            //
            TabPage schedulingTab = new TabPage("ScheduledSync");
            //
            //
            //
            Label lblScheduledSource = new Label();
            lblScheduledSource.Text = "Source Drive:";
            lblScheduledSource.Location = new Point(16, 170);
            lblScheduledSource.AutoSize = true;
            schedulingTab.Controls.Add(lblScheduledSource);
            comboScheduledSource = new ComboBox();
            comboScheduledSource.DropDownStyle = ComboBoxStyle.DropDownList;
            comboScheduledSource.Location = new Point(110, 165);
            comboScheduledSource.Size = new Size(100, 28);
            schedulingTab.Controls.Add(comboScheduledSource);
            Label lblScheduledDest = new Label();
            lblScheduledDest.Text = "Destination Drive:";
            lblScheduledDest.Location = new Point(230, 170);
            lblScheduledDest.AutoSize = true;
            schedulingTab.Controls.Add(lblScheduledDest);
            comboScheduledDest = new ComboBox();
            comboScheduledDest.DropDownStyle = ComboBoxStyle.DropDownList;
            comboScheduledDest.Location = new Point(350, 165);
            comboScheduledDest.Size = new Size(100, 28);
            schedulingTab.Controls.Add(comboScheduledDest);
            //
            // Enable Scheduling Checkbox
            //
            chkScheduleEnabled = new CheckBox();
            chkScheduleEnabled.Text = "Enable Scheduling";
            chkScheduleEnabled.Location = new Point(16, 18);
            chkScheduleEnabled.AutoSize = true;
            schedulingTab.Controls.Add(chkScheduleEnabled);
            //
            // Days of Week Checkboxes (Sunday to Saturday)
            //
            dayChecks = new CheckBox[7];
            string[] dayNames = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            for (int i = 0; i < 7; i++)
            {
                dayChecks[i] = new CheckBox();
                dayChecks[i].Text = dayNames[i];
                dayChecks[i].Location = new Point(16 + i * 60, 50);
                dayChecks[i].AutoSize = true;
                schedulingTab.Controls.Add(dayChecks[i]);
            }
            //
            // Scheduled Time Picker
            //
            Label lblTime = new Label();
            lblTime.Text = "Scheduled Time:";
            lblTime.Location = new Point(16, 90);
            lblTime.AutoSize = true;
            schedulingTab.Controls.Add(lblTime);
            timePickerScheduledTime = new DateTimePicker();
            timePickerScheduledTime.Format = DateTimePickerFormat.Time;
            timePickerScheduledTime.ShowUpDown = true;
            timePickerScheduledTime.Location = new Point(130, 85);
            schedulingTab.Controls.Add(timePickerScheduledTime);
            //
            // Frequency Dropdown
            //
            Label lblFrequency = new Label();
            lblFrequency.Text = "Frequency:";
            lblFrequency.Location = new Point(16, 130);
            lblFrequency.AutoSize = true;
            schedulingTab.Controls.Add(lblFrequency);
            cmbFrequency = new ComboBox();
            cmbFrequency.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFrequency.Location = new Point(130, 125);
            cmbFrequency.Size = new Size(120, 28);
            cmbFrequency.Items.AddRange(new string[] { "Daily", "Weekly", "Biweekly", "Monthly" });
            cmbFrequency.SelectedIndex = 1; // default Weekly
            schedulingTab.Controls.Add(cmbFrequency);
            //
            //Save Button
            //
            btnSaveSchedule = new Button();
            btnSaveSchedule.Text = "Save Settings";
            btnSaveSchedule.Location = new Point(16, 180);
            btnSaveSchedule.AutoSize = true;
            btnSaveSchedule.Click += btnSaveSchedule_Click;
            schedulingTab.Controls.Add(btnSaveSchedule);
            //
            // Add the new tab to the TabControl
            //
            tabControl.TabPages.Add(schedulingTab);


            this.Controls.Add(tabControl);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox comboSource;
        private System.Windows.Forms.ComboBox comboDest;
        private System.Windows.Forms.Button btnSyncNow;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button btnRefreshDrives;
        private System.Windows.Forms.CheckBox chkScheduleEnabled;
        private System.Windows.Forms.DateTimePicker timePickerScheduledTime;
        private System.Windows.Forms.ComboBox cmbFrequency;
        private System.Windows.Forms.CheckBox[] dayChecks;
        private System.Windows.Forms.Button btnSaveSchedule;
        private System.Windows.Forms.ComboBox comboScheduledSource;
        private System.Windows.Forms.ComboBox comboScheduledDest;


        
    }
}