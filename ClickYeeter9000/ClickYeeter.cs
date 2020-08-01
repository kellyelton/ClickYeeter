using ClickYeeter9000.Components;
using System;
using System.ComponentModel;
using System.IO;
using System.Media;

namespace ClickYeeter9000
{
    public class ClickYeeter : ModelBase {
        public Settings Settings { get; }

        public Theme Theme {
            get => _theme;
            private set => SetAndNotify(ref _theme, value);
        }

        public Recorder Recorder { get; }

        public RecordPlayer Player { get; }

        public Version Version{ get; }

        public string VersionString { get; }

        private Theme _theme;

        private readonly DirectoryInfo _themesDirectory;

        public ClickYeeter(Settings settings, string themesDirectory) {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            if (string.IsNullOrWhiteSpace(themesDirectory)) throw new ArgumentNullException(nameof(themesDirectory));

            _themesDirectory = new DirectoryInfo(themesDirectory);

            if (!_themesDirectory.Exists) throw new DirectoryNotFoundException($"Path '{_themesDirectory.FullName}' does not exist.");

            var ass = typeof(ClickYeeter).Assembly;
            Version = ass.GetName().Version;
            VersionString = $"v{Version} ⁞ Braden";

            Settings.PropertyChanged += Settings_PropertyChanged;

            Recorder = new Recorder();

            Recorder.PropertyChanged += Recorder_PropertyChanged;

            Player = new RecordPlayer();

            Player.PropertyChanged += Player_PropertyChanged;

            UpdateTheme();

            PlaySound(Theme.StartSound);
        }

        private void PlaySound(Stream audioStream) {
            using (audioStream) {
                if (Settings.SoundEnabled) {
                    using var player = new SoundPlayer(audioStream);

                    player.Load();

                    player.Play();
                }
            }
        }

        private void UpdateTheme() {
            if (Settings.Theme == null) throw new InvalidOperationException("Theme set to null");

            var themePath = Path.Combine(_themesDirectory.FullName, Settings.Theme);

            Theme = Theme.Load(themePath);
        }

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(Settings.Theme)) {
                UpdateTheme();
            }
        }

        private void Recorder_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(Recorder.IsEnabled)) {
                if (Recorder.IsEnabled) {
                    Player.IsEnabled = false; // stop playback

                    PlaySound(Theme.YeetOnSound);
                } else {
                    Player.Record = Recorder.Record;

                    PlaySound(Theme.YeetOffSound);
                }
            }
        }

        private void Player_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(Player.IsEnabled)) {
                if (Player.IsEnabled) {
                    Recorder.IsEnabled = false; // stop recording

                    PlaySound(Theme.YeetOnSound);
                } else {
                    PlaySound(Theme.YeetOffSound);
                }
            }
        }
    }
}
