using System; 
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text.Json;


namespace DriveMirror
{
    public partial class Form1 : Form
    {
        private string sourceDrive;
        private string destinationDrive;
        private FileSystemWatcher driveWatcher;
        private Scheduler _scheduler;
        private ScheduleManager _scheduleManager;
        private ToolStripMenuItem schedulingToggleMenuItem;

        public Form1()
        {
            InitializeComponent();
            InitializeTrayIcon();
            LoadDriveDropdowns();
            LoadDriveDropdownsForSchedule();
            
        _scheduleManager = new ScheduleManager();
        InitializeScheduler();
        LoadScheduleSettings();

            // Set custom icon
            this.Icon = new Icon("DriveMirror.ico");
        }

        private void LoadDriveDropdowns()
        {
            comboSource.Items.Clear();
            comboDest.Items.Clear();

            foreach (var drive in DriveInfo.GetDrives())
            {
                // Include both Fixed and Removable drives
                if (drive.IsReady &&
                    (drive.DriveType == DriveType.Fixed || drive.DriveType == DriveType.Removable))
                {
                    comboSource.Items.Add(drive.Name);
                    comboDest.Items.Add(drive.Name);
                }
            }

            comboSource.SelectedIndex = -1;
            comboDest.SelectedIndex = -1;

        }

        private void btnSyncNow_Click(object sender, EventArgs e)
        {
            StartSync();
        }

        private void StartSync(string scheduledSourceDrive = null, string scheduledDestDrive = null)
        {
            sourceDrive = scheduledSourceDrive ?? comboSource.SelectedItem?.ToString();
            destinationDrive = scheduledDestDrive ?? comboDest.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(sourceDrive) || string.IsNullOrEmpty(destinationDrive))
            {
                MessageBox.Show("Please select both source and destination drives.");
                return;
            }

            Logger.Log($"Sync requested. Source: {sourceDrive}, Destination: {destinationDrive}");
            StartRobocopy();
        }

        private void btnRefreshDrives_Click(object sender, EventArgs e)
        {
            Logger.Log("Refreshing drive lists...");
            LoadDriveDropdowns();
        }


        private void StartRobocopy()
        {
            var args = $"{sourceDrive} {destinationDrive} /MIR /XO /Z /B /R:1 /W:30 /XD "$RECYCLE.BIN" "System Volume Information" ";
            Logger.Log("Starting Robocopy with args: " + args);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "robocopy",
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        string line = e.Data.Trim();

                        // Filter out Robocopy banner/header lines
                        if (string.IsNullOrWhiteSpace(line)) return;
                        if (line.StartsWith("----") || line.StartsWith("ROBOCOPY") || line.StartsWith("Started") ||
                            line.StartsWith("Source") || line.StartsWith("Dest") || line.StartsWith("Files") ||
                            line.StartsWith("Options") || line.StartsWith("   ") || line.StartsWith("\t"))
                        {
                            return;
                        }

                        AppendLog(line);
                        Logger.Log(line);
                    }
                };

                try
                {
                    process.Start();
                    process.BeginOutputReadLine();
                }
                catch (Exception ex)
                {
                    Logger.Log("Failed to start DriveMirror: " + ex.Message);
                    MessageBox.Show("Failed to start DriveMirror:\n" + ex.Message);
                }
            }

        private void AppendLog(string text)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => txtLog.AppendText(text + Environment.NewLine)));
            }
            else
            {
                txtLog.AppendText(text + Environment.NewLine);
            }
        }

        private void InitializeTrayIcon()
        {
            notifyIcon1.Visible = true;
            notifyIcon1.Icon = new Icon(@"DriveMirror.ico"); 
            notifyIcon1.Text = "Drive Mirror";
            notifyIcon1.DoubleClick += (s, e) => { this.Show(); this.WindowState = FormWindowState.Normal; };

            var menu = new ContextMenuStrip();
            menu.Items.Add("Open", null, (s, e) => { this.Show(); this.WindowState = FormWindowState.Normal; });
            menu.Items.Add("Sync Now", null, (s, e) => btnSyncNow.PerformClick());
            schedulingToggleMenuItem = new ToolStripMenuItem("Enable Scheduling");
            schedulingToggleMenuItem.CheckOnClick = true;
            schedulingToggleMenuItem.CheckedChanged += (s, e) =>
            {
                if (schedulingToggleMenuItem.Checked)
                    _scheduler.Start();
                else
                    _scheduler.Stop();
            };
            menu.Items.Add(schedulingToggleMenuItem);
            menu.Items.Add("Exit", null, (s, e) => Application.Exit());
            notifyIcon1.ContextMenuStrip = menu;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }
        private void InitializeScheduler()
        {
            _scheduleManager = new ScheduleManager();
            _scheduler = new Scheduler(_scheduleManager);
            _scheduler.OnScheduledSync += RunSync;  // Hook your sync method here
            _scheduler.Start();
        }

        private void SchedulingToggleMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            bool isEnabled = schedulingToggleMenuItem.Checked;

            _scheduleManager.Settings.IsEnabled = isEnabled;
            _scheduleManager.Save();

            if (isEnabled)
                _scheduler.Start();
            else
                _scheduler.Stop();
        }

        private void RunSync()
        {
            var settings = _scheduleManager.Settings;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                    StartSync(settings.SourceDrive, settings.DestinationDrive)
                ));
            }
            else
            {
                StartSync(settings.SourceDrive, settings.DestinationDrive);
            }
        }

        private void btnSaveSchedule_Click(object sender, EventArgs e)
        {
            var settings = _scheduleManager.Settings;

            settings.IsEnabled = chkScheduleEnabled.Checked;
            settings.ScheduledTime = timePickerScheduledTime.Value.TimeOfDay;

            for (int i = 0; i < 7; i++)
            {
                settings.DaysOfWeek[i] = dayChecks[i].Checked;
            }

            settings.RunFrequency = (Frequency)Enum.Parse(typeof(Frequency), cmbFrequency.SelectedItem.ToString());
            settings.SourceDrive = comboScheduledSource.SelectedItem?.ToString() ?? "";
            settings.DestinationDrive = comboScheduledDest.SelectedItem?.ToString() ?? "";

            settings.LastRunDate = DateTime.MinValue;

            _scheduleManager.Save();

            MessageBox.Show("Schedule settings saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _scheduler.Stop();
            _scheduler.Start();
        }

        private void LoadDriveDropdownsForSchedule()
        {
            comboScheduledSource.Items.Clear();
            comboScheduledDest.Items.Clear();

            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && (drive.DriveType == DriveType.Fixed || drive.DriveType == DriveType.Removable))
                {
                    comboScheduledSource.Items.Add(drive.Name);
                    comboScheduledDest.Items.Add(drive.Name);
                }
            }

            if (comboScheduledSource.Items.Count > 0)
                comboScheduledSource.SelectedIndex = 0;
            if (comboScheduledDest.Items.Count > 1)
                comboScheduledDest.SelectedIndex = 1;
        }
        private void LoadScheduleSettings()
        {
            string path = "scheduleSettings.json";
            if (!File.Exists(path))
                return;

            try
            {
                string json = File.ReadAllText(path);
                var settings = JsonSerializer.Deserialize<ScheduleSettings>(json);

                chkScheduleEnabled.Checked = settings.IsEnabled;
                timePickerScheduledTime.Value = DateTime.Today.Add(settings.ScheduledTime);
                cmbFrequency.SelectedItem = settings.RunFrequency.ToString();

                for (int i = 0; i < 7; i++)
                {
                    if (i < dayChecks.Length)
                        dayChecks[i].Checked = settings.DaysOfWeek[i];
                }

                // Restore saved drive selections
                comboScheduledSource.SelectedItem = settings.SourceDrive;
                comboScheduledDest.SelectedItem = settings.DestinationDrive;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load schedule settings: " + ex.Message);
            }
        }

    }
}