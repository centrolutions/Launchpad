using Midi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Launchpad.Devices
{
    public class LaunchpadDevice : ILaunchpadDevice
    {
        private InputDevice _InputDevice;
        private OutputDevice _OutputDevice;

        private bool _DoubleBuffered;
        private bool _DoubleBufferedState;

        private readonly ILaunchpadButton[] _Toolbar = new ILaunchpadButton[8];
        private readonly ILaunchpadButton[] _Side = new ILaunchpadButton[8];
        private readonly ILaunchpadButton[,] _Grid = new ILaunchpadButton[8, 8];

        public ILaunchpadButton[] ToolbarButtons { get => _Toolbar; }
        public ILaunchpadButton[] SidebarButtons { get => _Side; }

        public event EventHandler<ButtonPressEventArgs> ButtonPressed;

        public bool DeviceAttached
        {
            get { return _InputDevice != null; }
        }

        public LaunchpadDevice() : this(0) { }

        public LaunchpadDevice(int index)
        {
            InitialiseButtons();

            int i = 0;
            _InputDevice = InputDevice.InstalledDevices.Where(x => x.Name.Contains("Launchpad")).
                FirstOrDefault(x => i++ == index);
            i = 0;
            _OutputDevice = OutputDevice.InstalledDevices.Where(x => x.Name.Contains("Launchpad")).
                FirstOrDefault(x => i++ == index);

            _InputDevice?.Open();
            _OutputDevice?.Open();

            if (_InputDevice != null)
            {
                _InputDevice.StartReceiving(new Clock(120));
                _InputDevice.NoteOn += InputDevice_NoteOn;
                _InputDevice.ControlChange += InputDevice_ControlChange;
            }

            if (_OutputDevice != null)
                Reset();
        }

        private void InitialiseButtons()
        {
            for (int i = 0; i < 8; i++)
            {
                _Toolbar[i] = new LaunchpadButton(this, ButtonType.Toolbar, 104 + i);
                _Side[i] = new LaunchpadButton(this, ButtonType.Side, i * 16 + 8);
            }

            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    _Grid[x, y] = new LaunchpadButton(this, ButtonType.Grid, y * 16 + x);
        }

        private void StartDoubleBuffering()
        {
            _DoubleBuffered = true;
            _DoubleBufferedState = false;
            _OutputDevice.SendControlChange(Channel.Channel1, (Control)0, 32 | 16 | 1);
        }

        public void Refresh()
        {
            if (!_DoubleBufferedState)
                _OutputDevice.SendControlChange(Channel.Channel1, (Control)0, 32 | 16 | 4);
            else
                _OutputDevice.SendControlChange(Channel.Channel1, (Control)0, 32 | 16 | 1);
            _DoubleBufferedState = !_DoubleBufferedState;
        }

        private void EndDoubleBuffering()
        {
            _OutputDevice.SendControlChange(Channel.Channel1, (Control)0, 32 | 16);
            _DoubleBuffered = false;
        }

        public void Reset()
        {
            if (!DeviceAttached)
                return;

            _OutputDevice.SendControlChange(Channel.Channel1, (Control)0, 0);
            Buttons.ToList().ForEach(x => x.RedBrightness = x.GreenBrightness = ButtonBrightness.Off);
        }

        private void InputDevice_NoteOn(NoteOnMessage msg)
        {
            var button = GetButton(msg.Pitch);
            if (button == null)
                return;

            button.State = (ButtonPressState)msg.Velocity;

            if (ButtonPressed != null && button.State == ButtonPressState.Down)
            {
                if ((int)msg.Pitch % 16 == 8)
                    ButtonPressed.Invoke(this, new ButtonPressEventArgs((SideButton)((int)msg.Pitch / 16)));
                else
                    ButtonPressed.Invoke(this, new ButtonPressEventArgs((int)msg.Pitch % 16, (int)msg.Pitch / 16));
            }
        }

        private void InputDevice_ControlChange(ControlChangeMessage msg)
        {
            ToolbarButton toolbarButton = (ToolbarButton)((int)msg.Control - 104);

            var button = GetButton(toolbarButton);
            if (button == null)
                return;

            button.State = (ButtonPressState)msg.Value;
            if (ButtonPressed != null && button.State == ButtonPressState.Down)
            {
                ButtonPressed.Invoke(this, new ButtonPressEventArgs(toolbarButton));
            }
        }

        public ILaunchpadButton GetButton(ToolbarButton toolbarButton)
        {
            return _Toolbar[(int)toolbarButton];
        }

        public ILaunchpadButton GetButton(SideButton sideButton)
        {
            return _Side[(int)sideButton];
        }

        private ILaunchpadButton GetButton(Pitch pitch)
        {
            int x = (int)pitch % 16;
            int y = (int)pitch / 16;
            if (x < 8 && y < 8)
                return _Grid[x, y];
            else if (x == 8 && y < 8)
                return _Side[y];

            return null;
        }

        public bool DoubleBuffered
        {
            get { return _DoubleBuffered; }
            set
            {
                if (_DoubleBuffered)
                    EndDoubleBuffering();
                else
                    StartDoubleBuffering();
            }
        }

        public ILaunchpadButton this[int x, int y]
        {
            get { return _Grid[x, y]; }
        }

        public IEnumerable<ILaunchpadButton> Buttons
        {
            get
            {
                for (int y = 0; y < 8; y++)
                    for (int x = 0; x < 8; x++)
                        yield return _Grid[x, y];
            }
        }

        public OutputDevice OutputDevice
        {
            get { return _OutputDevice; }
        }
    }
}
