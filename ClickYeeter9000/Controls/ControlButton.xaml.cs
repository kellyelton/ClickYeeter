using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ClickYeeter9000.Controls
{
    public partial class ControlButton : UserControl
    {
        public Brush Accent {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

        public static readonly DependencyProperty AccentProperty =
            DependencyProperty.Register("Accent", typeof(Brush), typeof(ControlButton), new PropertyMetadata(Brushes.White));

        public string ControlText {
            get { return (string)GetValue(ControlTextProperty); }
            set { SetValue(ControlTextProperty, value); }
        }

        public static readonly DependencyProperty ControlTextProperty =
            DependencyProperty.Register(nameof(ControlText), typeof(string), typeof(ControlButton), new PropertyMetadata("?"));

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
            nameof(Click), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ControlButton));

        public event RoutedEventHandler Click {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        private bool _didMouseDown;

        public ControlButton() {
            InitializeComponent();
        }

        void RaiseClickEvent() {
            OnClick();

            RoutedEventArgs newEventArgs = new RoutedEventArgs(ClickEvent);
            RaiseEvent(newEventArgs);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            _didMouseDown = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e) {
            if (!_didMouseDown) return;

            _didMouseDown = false;

            if (!IsMouseOver) return;

            RaiseClickEvent();
        }

        protected virtual void OnClick() { }
    }
}
