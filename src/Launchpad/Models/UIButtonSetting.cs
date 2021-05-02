using Launchpad.Actions;
using Launchpad.Devices;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Launchpad.Models
{
    public class UIButtonSetting : ObservableObject
    {
        private ButtonType _ButtonType;
        public ButtonType ButtonType
        {
            get => _ButtonType;
            set => SetProperty(ref _ButtonType, value);
        }

        private int _SortOrder;
        public int SortOrder
        {
            get => _SortOrder;
            set => SetProperty(ref _SortOrder, value);
        }

        private int _X;
        public int X
        {
            get => _X;
            set => SetProperty(ref _X, value);
        }

        private int _Y;
        public int Y
        {
            get => _Y;
            set => SetProperty(ref _Y, value);
        }

        private ButtonColor _Color;
        public ButtonColor Color
        { 
            get => _Color; 
            set => SetProperty(ref _Color, value); 
        }

        private IAction _ActionWhenPressed;
        public IAction ActionWhenPressed 
        { 
            get => _ActionWhenPressed; 
            set => SetProperty(ref _ActionWhenPressed, value); 
        }
    }
}
