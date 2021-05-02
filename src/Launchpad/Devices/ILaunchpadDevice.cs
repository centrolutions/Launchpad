using Midi;
using Midi.Devices;
using System;
using System.Collections.Generic;

namespace Launchpad.Devices
{
    public interface ILaunchpadDevice
    {
        ILaunchpadButton this[int x, int y] { get; }

        IEnumerable<ILaunchpadButton> Buttons { get; }
        bool DoubleBuffered { get; set; }
        bool DeviceAttached { get; }
        IOutputDevice OutputDevice { get; }

        event EventHandler<ButtonPressEventArgs> ButtonPressed;
        event EventHandler<DeviceAttachedChangedEventArgs> DeviceAttachedChanged;

        ILaunchpadButton GetButton(SideButton sideButton);
        ILaunchpadButton GetButton(ToolbarButton toolbarButton);
        void Refresh();
        void Reset();
    }
}