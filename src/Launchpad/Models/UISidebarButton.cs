using Launchpad.Devices;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Launchpad.Models
{
    public class UISidebarButton : ObservableObject
    {
        private string _Name;
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        private int _SortOrder;
        public int SortOrder
        {
            get => _SortOrder;
            set => SetProperty(ref _SortOrder, value);
        }

        private ButtonColor _Color;
        public ButtonColor Color
        {
            get => _Color;
            set => SetProperty(ref _Color, value);
        }
    }
}
