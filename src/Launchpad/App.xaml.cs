using Launchpad.Actions;
using Launchpad.Devices;
using Launchpad.Models;
using Launchpad.Properties;
using Launchpad.Services;
using Launchpad.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.Configuration;
using System.Windows;

namespace Launchpad
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ViewModelLocator ViewModelLocator
        {
            get { return App.Current.TryFindResource("ViewModelLocator") as ViewModelLocator; }
        }
        public App()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<ILaunchpadDevice, LaunchpadDevice>()
                    .AddSingleton<IDialogService, DialogService>()
                    .AddSingleton<IButtonSettingsStore, ButtonSettingsStore>()
                    .AddSingleton<IActionFactory, ActionFactory>()
                    .AddSingleton<ApplicationSettingsBase, Settings>()
                    .AddSingleton<LaunchpadSettingsViewModel>()
                    .AddTransient<ButtonSettingsViewModel>()
                    .AddSingleton<ObsSettingsViewModel>()
                    .BuildServiceProvider()
                );
            ;
        }
    }
}
