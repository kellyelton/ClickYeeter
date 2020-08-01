using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace ClickYeeter9000
{
    public partial class MainWindow : Window
    {
        public ClickYeeter ClickYeeter { get; }

        public Theme Theme { get; }

        private readonly IKeyboardMouseEvents _hook;

        private YeetOverlay _overlay;

        private readonly static KeysConverter _keysConverter = new KeysConverter();

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

            clickYeeter.Player.PropertyChanged += Player_PropertyChanged;
            clickYeeter.Recorder.PropertyChanged += Recorder_PropertyChanged;
        }

        private Storyboard _fadeBorderInStoryboard;
        private Storyboard _fadeBorderOutStoryboard;

        protected override void OnActivated(EventArgs e) {
            _fadeBorderOutStoryboard?.Stop();

            var sb = new Storyboard();
            var anim = new DoubleAnimation(0.2, 1, TimeSpan.FromSeconds(0.25), FillBehavior.HoldEnd);
            sb.Children.Add(anim);

            Storyboard.SetTarget(sb, MainBorder);
            Storyboard.SetTargetProperty(sb, new PropertyPath("BorderOpacity"));

            sb.Begin();

            _fadeBorderInStoryboard = sb;

            base.OnActivated(e);
        }

        protected override void OnDeactivated(EventArgs e) {
            _fadeBorderInStoryboard?.Stop();

            var sb = new Storyboard();
            var anim = new DoubleAnimation(1, 0.2, TimeSpan.FromSeconds(0.25), FillBehavior.HoldEnd);
            sb.Children.Add(anim);

            Storyboard.SetTarget(sb, MainBorder);
            Storyboard.SetTargetProperty(sb, new PropertyPath("BorderOpacity"));

            sb.Begin();

            _fadeBorderOutStoryboard = sb;

            base.OnDeactivated(e);
        }

        private void Player_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(ClickYeeter.Player.IsEnabled)) {
                if (ClickYeeter.Player.IsEnabled) {
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

        private void ToggleYeeting() {
            Dispatcher.VerifyAccess();

            ClickYeeter.Player.IsEnabled = !ClickYeeter.Player.IsEnabled;
        }

        private void ToggleRecording() {
            Dispatcher.VerifyAccess();

            ClickYeeter.Recorder.IsEnabled = !ClickYeeter.Recorder.IsEnabled;
        }

        private void _clickTimer_Elapsed(object sender, ElapsedEventArgs e) {
        }

        protected override void OnClosing(CancelEventArgs e) {
            ClickYeeter.Player.IsEnabled = false;
            ClickYeeter.Recorder.IsEnabled = false;

            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e) {
            System.Windows.Application.Current.Shutdown();

            base.OnClosed(e);
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
                    keys.Add(_keysConverter.ConvertToString(e.KeyCode));
                }
            }

            return string.Join(" + ", keys);
        }

        private void Hook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
            var keyString = GetKeyString(e);

            if (IsActive) {
                // UIElement.OnKeyDown/up etc don't catch all keys
                if (ToggleRecordingHotkeyTextBox.IsKeyboardFocusWithin) {
                    ToggleRecordingHotkeyTextBox.Text = keyString;

                    return;
                } else if (ToggleYeetingHotkeyTextBox.IsKeyboardFocusWithin) {
                    ToggleYeetingHotkeyTextBox.Text = keyString;

                    return;
                }
            }

            keyString = keyString.Replace(" ", "").ToLower();
            var recordHotKey = ClickYeeter.Settings.RecordHotKey.Replace(" ", "").ToLower();
            var yeetHotKey = ClickYeeter.Settings.YeetHotKey.Replace(" ", "").ToLower();

            if (keyString == recordHotKey) {
                ToggleRecording();
            } else if (keyString == yeetHotKey) {
                ToggleYeeting();
            }
        }
    }
}
