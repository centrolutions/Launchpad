using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.Actions
{
    public class ObsChangeSceneAction : ObsAction
    {
        private string _SceneName;
        public string SceneName
        {
            get => _SceneName;
            set => SetProperty(ref _SceneName, value);
        }

        public ObsChangeSceneAction() : base()
        {
            Name = "OBS Change Scene";
        }

        public override void ClearSettings()
        {
            SceneName = default;
        }

        public override void Execute()
        {
            try
            {
                Connect();
                _Obs.SetCurrentScene(SceneName);
                Disconnect();
            }
            catch { } //TODO: log errors
        }
    }
}
