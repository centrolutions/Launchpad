using Launchpad.Models;

namespace Launchpad.Services
{
    public interface IDialogService
    {
        bool? ShowButtonSettingsDialog(UIButtonSetting settings);
        bool? ShowObsSettingsDialog();
        string ShowOpenFileDialog(string fileTypeFilter);
        string ShowSaveFileDialog(string fileTypeFilter);
        bool ShowYesNoDialog(string question, string questionTitle);
    }
}
