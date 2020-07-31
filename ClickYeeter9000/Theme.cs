using System;
using System.IO;
using System.Windows.Media;

namespace ClickYeeter9000
{
    public class Theme : ModelBase
    {
        public static string DefaultThemesPath { get; set; }

        static Theme() {
            var assLocation = typeof(Theme).Assembly.Location;

            var assDir = Directory.GetParent(assLocation);

            DefaultThemesPath = Path.Combine(assDir.FullName, "Resources", "Themes");
        }

        public Stream YeetOnSound => File.OpenRead(Path.Combine(_path.FullName, "yeeton.wav"));

        public Stream YeetOffSound => File.OpenRead(Path.Combine(_path.FullName, "yeetoff.wav"));

        public Stream StartSound => File.OpenRead(Path.Combine(_path.FullName, "start.wav"));

        public Brush Background { get; } = new SolidColorBrush(Color.FromArgb(255, 29, 30, 36));

        public Color BorderColor { get; }

        public Brush Border { get; }

        private readonly DirectoryInfo _path;

        public Theme(DirectoryInfo path) {
            _path = path ?? throw new ArgumentNullException(nameof(path));

            BorderColor = Color.FromArgb(255, 137, 213, 255);
            Border = new SolidColorBrush(BorderColor);
        }

        public static Theme Load(string path) {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(path);

            var directory = new DirectoryInfo(path);

            if (!directory.Exists) throw new DirectoryNotFoundException($"Path '{path}' does not exist.");

            return new Theme(directory);
        }
    }
}
