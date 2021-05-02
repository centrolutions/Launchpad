using Launchpad.Devices;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Launchpad.Models
{
    public class UIGridButton: ObservableObject
    {

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
    }
}
