using Launchpad.Devices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Launchpad.Models
{
    public class ButtonSettingsStore : IButtonSettingsStore
    {
        private string ProfileFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "settings.json");

        public List<UIButtonSetting> AllSettings { get; private set; }

        public ButtonSettingsStore()
        {
            AllSettings = new List<UIButtonSetting>();
            EnsureProfileExists();
            OpenSettings(ProfileFilePath);
        }
        public UIButtonSetting GetButtonSetting(ButtonType type, int sequence = 0, int x = 0, int y = 0)
        {
            var found = AllSettings.Where(s => s.ButtonType == type && ((type == ButtonType.Grid && s.X == x && s.Y == y) || (type != ButtonType.Grid && s.SortOrder == sequence))).FirstOrDefault();
            if (found == null)
                return new UIButtonSetting() { ButtonType = type, SortOrder = sequence, X = x, Y = y, Color = ButtonColor.None };

            return found;
        }

        public void SetButtonSetting(UIButtonSetting setting)
        {
            if (!AllSettings.Contains(setting))
                AllSettings.Add(setting);
            SaveAllSettings();
        }

        public void SaveAllSettings()
        {
            SaveCurrentSettings(ProfileFilePath);
        }

        public void OpenSettings(string filePath)
        {
            var jsonSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            try
            {
                var results = JsonConvert.DeserializeObject<List<UIButtonSetting>>(File.ReadAllText(filePath), jsonSettings);
                AllSettings = results;
            }
            catch (Exception ex)
            {
                //TODO: log

                AllSettings = new List<UIButtonSetting>();
            }
        }

        public void SaveCurrentSettings(string filePath)
        {
            var jsonSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            try
            {
                var json = JsonConvert.SerializeObject(AllSettings, jsonSettings);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                //TODO: log

            }
        }

        private void EnsureProfileExists()
        {
            if (!File.Exists(ProfileFilePath))
                File.WriteAllText(ProfileFilePath, "[]");
        }
    }
}
