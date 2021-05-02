using System.Diagnostics;
using System.Windows;

namespace Launchpad
{
    /// <summary>
    /// Interaction logic for ButtonSettings.xaml
    /// </summary>
    public partial class ButtonSettings : Window
    {
        public ButtonSettings()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
