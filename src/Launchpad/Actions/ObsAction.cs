using Launchpad.Properties;
using Launchpad.Services;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using OBSWebsocketDotNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.Actions
{
    public abstract class ObsAction : ActionBase
    {
        protected OBSWebsocket _Obs;

        public RelayCommand ShowObsSettingsCommand { get; }

        public ObsAction()
        {
            _Obs = new OBSWebsocket();
            ShowObsSettingsCommand = new RelayCommand(ShowObsSettings);
        }

        private void ShowObsSettings()
        {
            var dialogs = Ioc.Default.GetService<IDialogService>();
            dialogs.ShowObsSettingsDialog();
        }

        protected void Connect()
        {
            var settings = Ioc.Default.GetService<ApplicationSettingsBase>();
            _Obs.Connect(settings["OBSAddress"] as string, settings["OBSPassword"] as string);
        }

        protected void Disconnect()
        {
            _Obs.Disconnect();
        }
    }
}
