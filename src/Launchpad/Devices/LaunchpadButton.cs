using Midi.Enums;

namespace Launchpad.Devices
{
    public class LaunchpadButton : ILaunchpadButton
    {
        private ILaunchpadDevice _LaunchpadDevice;
        private ButtonBrightness _RedBrightness, _GreenBrightness;
        private ButtonPressState _State;

        private ButtonType _Type;
        private int _Index;

        internal LaunchpadButton(ILaunchpadDevice launchpadDevice, ButtonType type, int index)
        {
            _LaunchpadDevice = launchpadDevice;
            _Type = type;
            _Index = index;
        }

        public void TurnOnLight()
        {
            SetBrightness(ButtonBrightness.Full, ButtonBrightness.Full);
        }

        public void TurnOffLight()
        {
            SetBrightness(ButtonBrightness.Off, ButtonBrightness.Off);
        }

        public void SetBrightness(ButtonBrightness red, ButtonBrightness green)
        {
            if (!_LaunchpadDevice.DeviceAttached || (_RedBrightness == red && _GreenBrightness == green))
                return;

            _RedBrightness = red;
            _GreenBrightness = green;

            int vel = ((int)_GreenBrightness << 4) | (int)_RedBrightness;

            if (!_LaunchpadDevice.DoubleBuffered)
                vel |= 12;

            SetLED(vel);
        }

        private void SetLED(int value)
        {
            if (_Type == ButtonType.Toolbar)
                _LaunchpadDevice.OutputDevice.SendControlChange(Channel.Channel1, (Control)_Index, value);
            else
                _LaunchpadDevice.OutputDevice.SendNoteOn(Channel.Channel1, (Pitch)_Index, value);
        }

        public ButtonBrightness RedBrightness
        {
            get { return _RedBrightness; }
            set { _RedBrightness = value; }
        }

        public ButtonBrightness GreenBrightness
        {
            get { return _GreenBrightness; }
            set { _GreenBrightness = value; }
        }

        public ButtonPressState State
        {
            get { return _State; }
            set { _State = value; }
        }
    }
}
