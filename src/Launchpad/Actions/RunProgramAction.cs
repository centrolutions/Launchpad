using Launchpad.Services;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System.Diagnostics;

namespace Launchpad.Actions
{
    public class RunProgramAction : ActionBase
    {
        private string _ExePath;
        public string ExePath
        {
            get => _ExePath;
            set => SetProperty(ref _ExePath, value);
        }

        public RelayCommand PickFileCommand { get; }

        public RunProgramAction()
        {
            Name = "Run a Program";
            PickFileCommand = new RelayCommand(PickFile);
        }
        public override void Execute()
        {
            try
            {
                Process.Start(ExePath);
            }
            catch { }
        }

        public override void ClearSettings()
        {
            ExePath = default;
        }


        private void PickFile()
        {
            var dialogs = Ioc.Default.GetService<IDialogService>();
            var filePath = dialogs.ShowOpenFileDialog("Executables (*.exe)|*.exe|Batch Files (*.bat)|*.bat|Commands (*.com)|*.com");
            if (!string.IsNullOrWhiteSpace(filePath))
                ExePath = filePath;
        }

    }
}
