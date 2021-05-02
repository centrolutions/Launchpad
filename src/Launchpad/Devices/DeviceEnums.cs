namespace Launchpad.Devices
{
	public enum ButtonType { Grid, Toolbar, Side }
	public enum ButtonBrightness { Off, Low, Medium, Full }
	public enum ButtonPressState { Up = 0, Down = 127 }
	public enum ToolbarButton { Up, Down, Left, Right, Session, User1, User2, Mixer }
	public enum SideButton { Volume, Pan, SoundA, SoundB, Stop, TrackOn, Solo, Arm }
	public enum ButtonColor { None, Red, Green, Orange }
}
