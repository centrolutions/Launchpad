using Launchpad.Services;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace Launchpad.Actions
{
    public class PlaySoundAction : ActionBase
    {
        private MediaPlayer _Player;

        private string _SoundFilePath;
        public string SoundFilePath
        {
            get => _SoundFilePath;
            set => SetProperty(ref _SoundFilePath, value);
        }

        public RelayCommand PickFileCommand { get; }

        public PlaySoundAction()
        {
            PickFileCommand = new RelayCommand(PickFile);
            Name = "Play a Sound";
        }

        public override void Execute()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                InitPlayer();
                if (_Player.Position.TotalMilliseconds > 0)
                {
                    StopAndReset();
                    return;
                }
                _Player.Play();
            });
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(SoundFilePath))
            {
                _Player = null;
                InitPlayer();
            }
        }

        public override void ClearSettings()
        {
            SoundFilePath = default;
        }

        private void InitPlayer()
        {
            if (_Player != null)
                return;

            _Player = new MediaPlayer();
            _Player.MediaEnded += Player_MediaEnded;
            _Player.Open(new Uri(SoundFilePath));
        }

        private void Player_MediaEnded(object sender, EventArgs e)
        {
            StopAndReset();
        }

        private void StopAndReset()
        {
            _Player.Stop();
            _Player.Position = new TimeSpan(0);
        }

        private void PickFile()
        {
            var dialogs = Ioc.Default.GetService<IDialogService>();
            var filePath = dialogs.ShowOpenFileDialog("WAV (*.wav)|*.wav|MP3 (*.mp3)|*.mp3");
            if (!string.IsNullOrWhiteSpace(filePath))
                SoundFilePath = filePath;
        }
    }
}
