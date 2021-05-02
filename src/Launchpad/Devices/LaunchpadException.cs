using System;

namespace Launchpad.Devices
{
    public class LaunchpadException : Exception
	{
		public LaunchpadException() : base() { }
		public LaunchpadException(string message) : base(message) { }
	}
}
