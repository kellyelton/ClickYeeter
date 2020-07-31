using System.Windows;

namespace ClickYeeter9000
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e) {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var settings = Settings.Load(Settings.DefaultPath);

            var clickYeeter = new ClickYeeter(settings, Theme.DefaultThemesPath);

            var mainWindow = new MainWindow(clickYeeter);

            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
