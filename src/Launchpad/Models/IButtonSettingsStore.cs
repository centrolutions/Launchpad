using Launchpad.Devices;
using System.Collections.Generic;

namespace Launchpad.Models
{
    public interface IButtonSettingsStore
    {
        List<UIButtonSetting> AllSettings { get; }

        UIButtonSetting GetButtonSetting(ButtonType type, int sequence = 0, int x = 0, int y = 0);
        void OpenSettings(string filePath);
        void SaveAllSettings();
        void SaveCurrentSettings(string filePath);
        void SetButtonSetting(UIButtonSetting setting);
    }
}