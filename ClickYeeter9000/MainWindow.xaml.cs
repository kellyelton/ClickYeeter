//using GlobalHotKey;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace ClickYeeter9000
{
    public partial class MainWindow : Window
    {
        public ClickYeeter ClickYeeter { get; }

        public Theme Theme { get; }

        private readonly IKeyboardMouseEvents _hook;
        //private readonly List<HotKey> _registereHotKeys = new List<HotKey>();

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

            _hook = Hook.GlobalEvents();
            _hook.KeyDown += Hook_KeyDown;
            _hook.KeyUp += Hook_KeyUp;

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

        //private void Manager_KeyPressed(object sender, KeyPressedEventArgs e) {
        //    if(e.HotKey.Key == Key.F6) {
        //        if (e.HotKey.Modifiers.HasFlag(ModifierKeys.Shift)) {
        //            Dispatcher.InvokeAsync(ToggleRecording);
        //        } else {
        //            Dispatcher.InvokeAsync(ToggleYeeting);
        //        }
        //    }
        //}

        private void _clickTimer_Elapsed(object sender, ElapsedEventArgs e) {
            //Mouse.MouseEvent(Mouse.MouseEventFlags.LeftDown);

            //Mouse.MouseEvent(Mouse.MouseEventFlags.LeftUp);
        }

        protected override void OnClosing(CancelEventArgs e) {
            DisableYeeting();

            //foreach (var hotkey in _registereHotKeys) {
            //    _hotkeyManager.Unregister(hotkey);
            //}

            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e) {
            System.Windows.Application.Current.Shutdown();

            base.OnClosed(e);
        }

        private static string GetKeyString(System.Windows.Input.KeyEventArgs e) {
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

        private static string GetKeyString(System.Windows.Forms.KeyEventArgs e) {
            var keys = new List<string>();

            {
                var mods = e.Modifiers;

                if (mods.HasFlag(Keys.LWin) | mods.HasFlag(Keys.LWin)) {
                    keys.Add("WIN");
                } else {
                    if (mods.HasFlag(Keys.Control)) {
                        keys.Add("CTRL");
                    }

                    if (mods.HasFlag(Keys.Shift)) {
                        keys.Add("SHIFT");
                    }

                    if (mods.HasFlag(Keys.Alt)) {
                        keys.Add("ALT");
                    }
                }
            }

            {
                if (e.KeyCode >= Keys.NumLock && e.KeyCode <= Keys.D9) {
                    return string.Empty;
                } else if (e.KeyCode >= Keys.LWin && e.KeyCode <= Keys.Sleep) {
                    return string.Empty;
                } else {
                    keys.Add(_keyConverter.ConvertToString(e.KeyCode));
                }
            }

            return string.Join(" + ", keys);
        }

        private void Hook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (!this.IsActive) return;

            if (ToggleRecordingHotkeyTextBox.IsKeyboardFocusWithin) {
                ToggleRecordingHotkeyTextBox.Text = GetKeyString(e);
            } else if (ToggleYeetingHotkeyTextBox.IsKeyboardFocusWithin) {
                ToggleYeetingHotkeyTextBox.Text = GetKeyString(e);
            }
        }

        private void Hook_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
            //throw new NotImplementedException();
        }

        private void ToggleRecordHotKeyTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            var tb = (System.Windows.Controls.TextBox)sender;

            tb.Text = GetKeyString(e);
        }

        private void ToggleYeetHotKeyTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            var tb = (System.Windows.Controls.TextBox)sender;

            tb.Text = GetKeyString(e);
        }
    }
}
