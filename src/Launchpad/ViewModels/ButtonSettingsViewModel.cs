using Launchpad.Actions;
using Launchpad.Devices;
using Launchpad.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Launchpad.ViewModels
{
    public class ButtonSettingsViewModel : ObservableObject
    {
        private readonly IActionFactory _ActionFactory;
        private UIButtonSetting _Settings;
        public UIButtonSetting Settings
        {
            get => _Settings;
        }

        public RelayCommand ClearSettingsCommand { get; }

        public ObservableCollection<string> Actions { get; private set; }

        private string _SelectedAction;
        public string SelectedAction
        {
            get => _SelectedAction;
            set => SetProperty(ref _SelectedAction, value);
        }

        private ButtonColor _Color;
        public ButtonColor Color
        {
            get => _Color;
            set => SetProperty(ref _Color, value);
        }

        public ButtonSettingsViewModel(IActionFactory actionFactory)
        {
            ClearSettingsCommand = new RelayCommand(ClearSettings);
            _ActionFactory = actionFactory;
            Actions = new ObservableCollection<string>(_ActionFactory.GetAllNames());
        }

        public void SetSettings(UIButtonSetting settings)
        {
            _Settings = settings;
            SelectedAction = (settings.ActionWhenPressed == null) ? null : settings.ActionWhenPressed.Name;
            Color = settings.Color;
            OnPropertyChanged(nameof(Settings));
        }

        public UIButtonSetting GetSettings()
        {
            _Settings.Color = Color;
            return _Settings;
        }

        private void ClearSettings()
        {
            SelectedAction = null;
            Color = ButtonColor.None;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(SelectedAction))
            {
                if (SelectedAction != null && _Settings?.ActionWhenPressed?.Name != SelectedAction)
                {
                    _Settings.ActionWhenPressed = _ActionFactory.CreateNewByName(SelectedAction);
                    OnPropertyChanged(nameof(Settings));
                }
            }
        }
    }
}
