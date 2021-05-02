using System.Windows.Forms;

namespace Launchpad.Actions
{
    public class KeyboardMacroAction : ActionBase
    {
        private string _Keys;
        public string Keys
        {
            get => _Keys;
            set => SetProperty(ref _Keys, value);
        }

        public KeyboardMacroAction()
        {
            this.Name = "Keyboard Macro";
        }

        public override void Execute()
        {
            try
            {
                SendKeys.SendWait(Keys);
            }
            catch { }
        }

        public override void ClearSettings()
        {
            Keys = default;
        }
    }
}
