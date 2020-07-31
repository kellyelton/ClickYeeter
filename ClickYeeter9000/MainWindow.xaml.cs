using GlobalHotKey;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClickYeeter9000
{
    public partial class MainWindow : Window
    {
        public ClickYeeter ClickYeeter { get; }

        public Theme Theme { get; }

        private readonly HotKeyManager _hotkeyManager;
        private readonly List<HotKey> _registereHotKeys = new List<HotKey>();

        private YeetOverlay _overlay;

        private readonly static KeyConverter _keyConverter = new KeyConverter();

        [Obsolete("Used for the WPF designer only")]
        public MainWindow() {
            if (!DesignerProperties.GetIsInDesignMode(this)) {
                throw new InvalidOperationException("Can not use this constructor unless you're in design mode.");
            }
        }

        public MainWindow(ClickYeeter clickYeeter) {
            DataContext = ClickYeeter = clickYeeter ?? throw new ArgumentNullException(nameof(clickYeeter));

            InitializeComponent();

            _hotkeyManager = new HotKeyManager();

            _hotkeyManager.KeyPressed += Manager_KeyPressed;

            var hotkey = new HotKey(Key.F6, ModifierKeys.None);

            _hotkeyManager.Register(hotkey);

            var hotkey2 = new HotKey(Key.F6, ModifierKeys.Shift);

            _hotkeyManager.Register(hotkey2);

            clickYeeter.PropertyChanged += ClickYeeter_PropertyChanged;
            clickYeeter.Recorder.PropertyChanged += Recorder_PropertyChanged;

        }

        private void ClickYeeter_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(ClickYeeter.IsEnabled)) {
                if (ClickYeeter.IsEnabled) {
                    var overlay = _overlay = new YeetOverlay();
                    overlay.Show();
                } else {
                    _overlay.Close();

                    _overlay = null;
                }
            }
        }

        private void Recorder_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(ClickYeeter.Recorder.IsEnabled)) {
                if (ClickYeeter.Recorder.IsEnabled) {
                    var overlay = _overlay = new YeetOverlay();
                    overlay.Show();
                } else {
                    _overlay.Close();

                    _overlay = null;
                }
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();

            base.OnMouseDown(e);
        }

        private void EnableYeeting() {
            Dispatcher.VerifyAccess();

            ClickYeeter.IsEnabled = true;
        }

        private void DisableYeeting() {
            Dispatcher.VerifyAccess();

            ClickYeeter.IsEnabled = false;
        }

        private void ToggleYeeting() {
            Dispatcher.VerifyAccess();

            ClickYeeter.IsEnabled = !ClickYeeter.IsEnabled;
        }

        private void ToggleRecording() {
            Dispatcher.VerifyAccess();

            ClickYeeter.Recorder.IsEnabled = !ClickYeeter.Recorder.IsEnabled;
        }

        private void Manager_KeyPressed(object sender, KeyPressedEventArgs e) {
            if(e.HotKey.Key == Key.F6) {
                if (e.HotKey.Modifiers.HasFlag(ModifierKeys.Shift)) {
                    Dispatcher.InvokeAsync(ToggleRecording);
                } else {
                    Dispatcher.InvokeAsync(ToggleYeeting);
                }
            }
        }

        private void _clickTimer_Elapsed(object sender, ElapsedEventArgs e) {
            //Mouse.MouseEvent(Mouse.MouseEventFlags.LeftDown);

            //Mouse.MouseEvent(Mouse.MouseEventFlags.LeftUp);
        }

        protected override void OnClosing(CancelEventArgs e) {
            DisableYeeting();

            foreach (var hotkey in _registereHotKeys) {
                _hotkeyManager.Unregister(hotkey);
            }

            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e) {
            Application.Current.Shutdown();

            base.OnClosed(e);
        }

        private static string GetKeyString(KeyEventArgs e) {
            var keys = new List<string>();

            {
                var mods = Keyboard.Modifiers;

                if (mods.HasFlag(ModifierKeys.Windows)) {
                    keys.Add("WIN");
                } else {
                    if (mods.HasFlag(ModifierKeys.Control)) {
                        keys.Add("CTRL");
                    }

                    if (mods.HasFlag(ModifierKeys.Shift)) {
                        keys.Add("SHIFT");
                    }

                    if (mods.HasFlag(ModifierKeys.Alt)) {
                        keys.Add("ALT");
                    }
                }
            }

            {
                if (e.Key >= Key.LeftShift && e.Key <= Key.RightAlt) {
                    return string.Empty;
                } else if (e.Key == Key.LWin || e.Key == Key.RWin || e.Key == Key.System) {
                    return string.Empty;
                } else {
                    keys.Add(_keyConverter.ConvertToString(e.Key));
                }
            }

            return string.Join(" + ", keys);
        }

        private void ToggleRecordHotKeyTextBox_KeyDown(object sender, KeyEventArgs e) {
            var tb = (TextBox)sender;

            tb.Text = GetKeyString(e);
        }

        private void ToggleYeetHotKeyTextBox_KeyDown(object sender, KeyEventArgs e) {
            var tb = (TextBox)sender;

            tb.Text = GetKeyString(e);
        }
    }
}
