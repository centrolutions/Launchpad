using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace Launchpad.ViewModels
{
    public class ViewModelLocator
    {
        public LaunchpadSettingsViewModel LaunchpadSettings
        {
            get
            {
                return Ioc.Default.GetService<LaunchpadSettingsViewModel>();
            }
        }

        public ButtonSettingsViewModel ButtonSettings
        {
            get
            {
                return Ioc.Default.GetService<ButtonSettingsViewModel>();
            }
        }

        public ObsSettingsViewModel ObsSettings
        {
            get
            {
                return Ioc.Default.GetService<ObsSettingsViewModel>();
            }
        }
    }
}
