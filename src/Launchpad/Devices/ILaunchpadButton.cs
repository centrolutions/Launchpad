namespace Launchpad.Devices
{
    public interface ILaunchpadButton
    {
        ButtonBrightness GreenBrightness { get; set; }
        ButtonBrightness RedBrightness { get; set; }
        ButtonPressState State { get; set; }

        void SetBrightness(ButtonBrightness red, ButtonBrightness green);
        void TurnOffLight();
        void TurnOnLight();
    }
}