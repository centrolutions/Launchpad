using Launchpad.ViewModels;
using System;
using System.Windows;
using System.Windows.Forms;

namespace Launchpad
{
    /// <summary>
    /// Interaction logic for LaunchpadSettings.xaml
    /// </summary>
    public partial class LaunchpadSettings : Window
    {
        private bool _ShouldQuit;
        public LaunchpadSettings()
        {
            InitializeComponent();
            SetupNotificationIcon();

        }

        private void SetupNotificationIcon()
        {
            var icon = new NotifyIcon();
            icon.Icon = new System.Drawing.Icon("Launchpad.ico");
            icon.Visible = true;
            icon.ContextMenu = new System.Windows.Forms.ContextMenu(new System.Windows.Forms.MenuItem[]
            {
                new System.Windows.Forms.MenuItem("Quit", NotifyIconContextMenu_QuitClicked)
            });
            
            icon.DoubleClick += Icon_DoubleClick;
        }

        private void NotifyIconContextMenu_QuitClicked(object sender, EventArgs e)
        {
            _ShouldQuit = true;
            this.Close();
        }

        private void Icon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_ShouldQuit)
            {
                e.Cancel = true;
                this.Hide();
                return;
            }

            ((LaunchpadSettingsViewModel)DataContext).ClosingCommand.Execute(null);

        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();
            base.OnStateChanged(e);
        }
    }
}
