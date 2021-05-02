using System;
using System.Collections.Generic;

namespace Launchpad.Devices
{
    public interface ILaunchpadDevice
    {
        LaunchpadButton this[int x, int y] { get; }

        IEnumerable<LaunchpadButton> Buttons { get; }
        bool DoubleBuffered { get; set; }
        bool DeviceAttached { get; }

        event EventHandler<ButtonPressEventArgs> ButtonPressed;

        LaunchpadButton GetButton(SideButton sideButton);
        LaunchpadButton GetButton(ToolbarButton toolbarButton);
        void Refresh();
        void Reset();
    }
}