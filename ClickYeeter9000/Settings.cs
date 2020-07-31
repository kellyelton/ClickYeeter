using Config.Net;
using System;
using System.IO;

namespace ClickYeeter9000
{
    public class Settings : ModelBase, ISettings
    {
        public string YeetHotKey {
            get => _settings.YeetHotKey;
            set {
                if (_settings.YeetHotKey == value) return;

                _settings.YeetHotKey = value;

                Notify();
            }
        }

        public string RecordHotKey {
            get => _settings.RecordHotKey;
            set {
                if (_settings.RecordHotKey == value) return;

                _settings.RecordHotKey = value;

                Notify();
            }
        }

        public string Theme {
            get => _settings.Theme;
            set {
                if (_settings.Theme == value) return;

                _settings.Theme = value;

                Notify();
            }
        }

        public bool SoundEnabled {
            get => _settings.SoundEnabled;
            set {
                if (_settings.SoundEnabled == value) return;

                _settings.SoundEnabled = value;

                Notify();
            }
        }

        private readonly ISettings _settings;

        public Settings(ISettings settings) {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public const string DefaultPath = "settings.ini";

        public static Settings Load(string path) {
            if (!File.Exists(path)) {
                using var _ = File.Create(path);
            }

            var settings = new ConfigurationBuilder<ISettings>()
                .UseIniFile(path)
                .Build();

            return new Settings(settings);
        }
    }
}
