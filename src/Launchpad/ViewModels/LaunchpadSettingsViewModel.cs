using Launchpad.Devices;
using Launchpad.Models;
using Launchpad.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Launchpad.ViewModels
{
    public class LaunchpadSettingsViewModel : ObservableObject
    {
        private readonly ILaunchpadDevice _Launchpad;
        private readonly IDialogService _DialogService;
        private readonly IButtonSettingsStore _ButtonSettings;

        public ObservableCollection<UIGridButton> GridButtons { get; }
        public ObservableCollection<UIToolbarButton> ToolbarButtons { get; }
        public ObservableCollection<UISidebarButton> SidebarButtons { get; }

        public RelayCommand<UIGridButton> OpenGridButtonSettingsCommand { get; }
        public RelayCommand<UIToolbarButton> OpenToolbarButtonSettingsCommand { get; }
        public RelayCommand<UISidebarButton> OpenSidebarButtonSettingsCommand { get; }
        public RelayCommand ClearAllSettingsCommand { get; }
        public RelayCommand ClosingCommand { get; }
        public RelayCommand OpenButtonSettingsFileCommand { get; }
        public RelayCommand SaveButtonSettingsFileCommand { get; }

        public bool _IsDeviceAttached;
        public bool IsDeviceAttached
        {
            get => _IsDeviceAttached;
            set => SetProperty(ref _IsDeviceAttached, value);
        }

        public LaunchpadSettingsViewModel(ILaunchpadDevice launchpad, IDialogService dialogService, IButtonSettingsStore buttonSettings)
        {
            _Launchpad = launchpad;
            _DialogService = dialogService;
            _ButtonSettings = buttonSettings;
            GridButtons = new ObservableCollection<UIGridButton>(GetGridButtons());
            ToolbarButtons = new ObservableCollection<UIToolbarButton>(GetToolbarButtons());
            SidebarButtons = new ObservableCollection<UISidebarButton>(GetSidebarButtons());

            OpenGridButtonSettingsCommand = new RelayCommand<UIGridButton>(OpenButtonSettings);
            OpenToolbarButtonSettingsCommand = new RelayCommand<UIToolbarButton>(OpenButtonSettings);
            OpenSidebarButtonSettingsCommand = new RelayCommand<UISidebarButton>(OpenButtonSettings);
            ClearAllSettingsCommand = new RelayCommand(ClearAllSettings);
            ClosingCommand = new RelayCommand(Closing);
            OpenButtonSettingsFileCommand = new RelayCommand(OpenButtonSettingsFile);
            SaveButtonSettingsFileCommand = new RelayCommand(SaveButtonSettingsFile);

            SetButtonColors();
            IsDeviceAttached = _Launchpad.DeviceAttached;
            _Launchpad.ButtonPressed += Launchpad_ButtonPressed;
            _Launchpad.DeviceAttachedChanged += Launchpad_DeviceAttachedChanged;
        }

        private void SaveButtonSettingsFile()
        {
            var filePath = _DialogService.ShowSaveFileDialog("JSON file (*.json)|*.json");
            if (!string.IsNullOrWhiteSpace(filePath))
                _ButtonSettings.SaveCurrentSettings(filePath);
        }

        private void OpenButtonSettingsFile()
        {
            var filePath = _DialogService.ShowOpenFileDialog("JSON file (*.json)|*.json");
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                _ButtonSettings.OpenSettings(filePath);
                _Launchpad.Reset();
                ClearUIButtonColors();
                SetButtonColors();
            }
        }

        private void Closing()
        {
            _Launchpad.Reset();
        }

        private void ClearAllSettings()
        {
            var shouldClear = _DialogService.ShowYesNoDialog("Are you sure you want to clear all of your presets and colors?", "Clear all presets?");
            if (!shouldClear)
                return;

            _ButtonSettings.AllSettings.Clear();
            _ButtonSettings.SaveAllSettings();
            _Launchpad.Reset();
            ClearUIButtonColors();
        }

        private void OpenButtonSettings(UIGridButton button)
        {
            var lpbutton = _Launchpad[button.X, button.Y];
            var setting = _ButtonSettings.GetButtonSetting(ButtonType.Grid, x: button.X, y: button.Y);

            var result = _DialogService.ShowButtonSettingsDialog(setting);
            button.Color = setting.Color;
            _ButtonSettings.SetButtonSetting(setting);
            SetButtonColor(setting, lpbutton);
        }

        private void OpenButtonSettings(Models.UIToolbarButton button)
        {
            var lpbutton = _Launchpad.GetButton((Devices.ToolbarButton)button.SortOrder);
            var setting = _ButtonSettings.GetButtonSetting(ButtonType.Toolbar, sequence: button.SortOrder);

            var result = _DialogService.ShowButtonSettingsDialog(setting);
            _ButtonSettings.SetButtonSetting(setting);
            button.Color = setting.Color;
            SetButtonColor(setting, lpbutton);
        }

        private void OpenButtonSettings(UISidebarButton button)
        {
            var lpbutton = _Launchpad.GetButton((SideButton)button.SortOrder);
            var setting = _ButtonSettings.GetButtonSetting(ButtonType.Side, sequence: button.SortOrder);

            var result = _DialogService.ShowButtonSettingsDialog(setting);
            button.Color = setting.Color;
            _ButtonSettings.SetButtonSetting(setting);
            SetButtonColor(setting, lpbutton);
        }

        private IEnumerable<UISidebarButton> GetSidebarButtons()
        {
            foreach (Devices.SideButton e in Enum.GetValues(typeof(SideButton)))
                yield return new UISidebarButton()
                {
                    Name = e.ToString(),
                    SortOrder = (int)e,
                };
        }

        private IEnumerable<UIGridButton> GetGridButtons()
        {
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    yield return new UIGridButton()
                    {
                        X = x,
                        Y = y,
                    };
        }
        private IEnumerable<Models.UIToolbarButton> GetToolbarButtons()
        {
            foreach (Devices.ToolbarButton e in Enum.GetValues(typeof(Devices.ToolbarButton)))
                yield return new Models.UIToolbarButton()
                {
                    Name = e.ToString(),
                    SortOrder = (int)e,
                };
        }

        private void ClearUIButtonColors()
        {
            foreach (var btn in GridButtons)
                btn.Color = ButtonColor.None;
            foreach (var btn in ToolbarButtons)
                btn.Color = ButtonColor.None;
            foreach (var btn in SidebarButtons)
                btn.Color = ButtonColor.None;
        }

        private void SetButtonColors()
        {
            foreach (var setting in _ButtonSettings.AllSettings)
            {
                ILaunchpadButton button = null;
                switch (setting.ButtonType)
                {
                    case ButtonType.Grid:
                        GridButtons.First(b => b.X == setting.X && b.Y == setting.Y).Color = setting.Color;
                        button = _Launchpad[setting.X, setting.Y];
                        break;
                    case ButtonType.Side:
                        SidebarButtons[setting.SortOrder].Color = setting.Color;
                        button = _Launchpad.GetButton((SideButton)setting.SortOrder);
                        break;
                    case ButtonType.Toolbar:
                        ToolbarButtons[setting.SortOrder].Color = setting.Color;
                        button = _Launchpad.GetButton((Devices.ToolbarButton)setting.SortOrder);
                        break;
                }

                if (button == null)
                    continue;

                SetButtonColor(setting, button);
            }
        }

        private void SetButtonColor(UIButtonSetting setting, ILaunchpadButton button)
        {
            switch (setting.Color)
            {
                case ButtonColor.None:
                    button.SetBrightness(ButtonBrightness.Off, ButtonBrightness.Off);
                    break;
                case ButtonColor.Green:
                    button.SetBrightness(ButtonBrightness.Off, ButtonBrightness.Full);
                    break;
                case ButtonColor.Red:
                    button.SetBrightness(ButtonBrightness.Full, ButtonBrightness.Off);
                    break;
                case ButtonColor.Orange:
                    button.SetBrightness(ButtonBrightness.Full, ButtonBrightness.Full);
                    break;
            }
        }

        private void Launchpad_ButtonPressed(object sender, ButtonPressEventArgs e)
        {
            UIButtonSetting setting = null;
            switch (e.Type)
            {
                case ButtonType.Grid:
                    setting = _ButtonSettings.GetButtonSetting(ButtonType.Grid, x: e.X, y: e.Y);
                    break;
                case ButtonType.Side:
                    setting = _ButtonSettings.GetButtonSetting(ButtonType.Side, (int)e.SidebarButton);
                    break;
                case ButtonType.Toolbar:
                    setting = _ButtonSettings.GetButtonSetting(ButtonType.Toolbar, (int)e.ToolbarButton);
                    break;
            }

            if ((setting?.ActionWhenPressed) == null)
                return;

            setting.ActionWhenPressed.Execute();
        }

        private void Launchpad_DeviceAttachedChanged(object sender, DeviceAttachedChangedEventArgs e)
        {
            IsDeviceAttached = e.NewValue;
            SetButtonColors();
        }
    }
}
