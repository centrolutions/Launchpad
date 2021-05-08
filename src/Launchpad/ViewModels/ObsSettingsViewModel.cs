using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.ViewModels
{
    public class ObsSettingsViewModel : ObservableObject
    {
        private readonly ApplicationSettingsBase _Settings;

        public string ObsAddress
        {
            get => (string)_Settings["OBSAddress"];
            set
            {
                if (value == (string)_Settings[ObsAddress])
                    return;

                OnPropertyChanging(nameof(ObsAddress));
                _Settings["OBSAddress"] = value;
                _Settings.Save();
                OnPropertyChanged(nameof(ObsAddress));
            }
        }

        public string ObsPassword
        {
            get => (string)_Settings["OBSPassword"];
            set
            {
                if (value == (string)_Settings["OBSPassword"])
                    return;

                OnPropertyChanging(nameof(ObsPassword));
                _Settings["OBSPassword"] = value;
                _Settings.Save();
                OnPropertyChanged(nameof(ObsPassword));
            }
        }

        public ObsSettingsViewModel(ApplicationSettingsBase settings)
        {
            _Settings = settings;
        }

    }
}
