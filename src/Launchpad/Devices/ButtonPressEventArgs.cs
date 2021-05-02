using System;

namespace Launchpad.Devices
{
    public class ButtonPressEventArgs : EventArgs
	{
		private ButtonType _Type;
		private ToolbarButton _ToolbarButton;
		private SideButton _SidebarButton;
		private int _X, _Y;

		public ButtonPressEventArgs(ToolbarButton toolbarButton)
		{
			_Type = ButtonType.Toolbar;
			_ToolbarButton = toolbarButton;
		}

		public ButtonPressEventArgs(SideButton sideButton)
		{
			_Type = ButtonType.Side;
			_SidebarButton = sideButton;
		}

		public ButtonPressEventArgs(int x, int y)
		{
			_Type = ButtonType.Grid;
			_X = x;
			_Y = y;
		}

		public ButtonType Type
		{
			get { return _Type; }
		}

		public ToolbarButton ToolbarButton
		{
			get { return _ToolbarButton; }
		}

		public SideButton SidebarButton
		{
			get { return _SidebarButton; }
		}

		public int X
		{
			get { return _X; }
		}

		public int Y
		{
			get { return _Y; }
		}
	}
}
