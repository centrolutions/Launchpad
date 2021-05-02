using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.Devices
{
    public class DeviceAttachedChangedEventArgs
    {
        public bool OldValue { get; }
        public bool NewValue { get; }
        public DeviceAttachedChangedEventArgs(bool oldValue, bool newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
