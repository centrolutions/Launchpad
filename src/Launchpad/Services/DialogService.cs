using Launchpad.Models;
using Launchpad.ViewModels;
using Microsoft.Win32;
using System.Windows;

namespace Launchpad.Services
{
    public class DialogService : IDialogService
    {
        public bool? ShowButtonSettingsDialog(UIButtonSetting settings)
        {
            var window = new ButtonSettings();
            window.Owner = App.Current.MainWindow;
            ((ButtonSettingsViewModel)window.DataContext).SetSettings(settings);
            var result = window.ShowDialog();
            ((ButtonSettingsViewModel)window.DataContext).GetSettings();
            return result;
        }

        public bool? ShowObsSettingsDialog()
        {
            var window = new ObsSettings();
            window.Owner = App.Current.MainWindow;
            var result = window.ShowDialog();

            return result;
        }

        public bool ShowYesNoDialog(string question, string questionTitle)
        {
            var result = MessageBox.Show(App.Current.MainWindow, question, questionTitle, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.None);
            return result == MessageBoxResult.Yes;
        }

        public string ShowSaveFileDialog(string fileTypeFilter)
        {
            var fileDialog = new SaveFileDialog() { Filter = fileTypeFilter };
            if (fileDialog.ShowDialog(App.Current.MainWindow) == true)
                return fileDialog.FileName;

            return null;
        }

        public string ShowOpenFileDialog(string fileTypeFilter)
        {
            var fileDialog = new OpenFileDialog() { Filter = fileTypeFilter };
            if (fileDialog.ShowDialog(App.Current.MainWindow) == true)
                return fileDialog.FileName;

            return null;
        }
    }
}
