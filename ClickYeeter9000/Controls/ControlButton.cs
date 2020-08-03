using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ClickYeeter9000.Controls
{
    public partial class ControlButton : Control
    {
        static ControlButton() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ControlButton), new FrameworkPropertyMetadata(typeof(ControlButton)));
        }

        public Brush HoverBackground {
            get { return (Brush)GetValue(HoverBackgroundProperty); }
            set { SetValue(HoverBackgroundProperty, value); }
        }

        public static readonly DependencyProperty HoverBackgroundProperty =
            DependencyProperty.Register(nameof(HoverBackground), typeof(Brush), typeof(ControlButton), new PropertyMetadata(Brushes.White));

        public Brush MouseDownBackground {
            get { return (Brush)GetValue(MouseDownBackgroundProperty); }
            set { SetValue(MouseDownBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MouseDownBackgroundProperty =
            DependencyProperty.Register(nameof(MouseDownBackground), typeof(Brush), typeof(ControlButton), new PropertyMetadata(Brushes.DarkGray));

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
            IsHitTestVisible = true;
            MouseEnter += (s, e) => VisualStateManager.GoToState(this, "Hover", false);
            MouseLeave += (s, e) => VisualStateManager.GoToState(this, "Normal", false);
        }

        void RaiseClickEvent() {
            OnClick();

            RoutedEventArgs newEventArgs = new RoutedEventArgs(ClickEvent);
            RaiseEvent(newEventArgs);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            e.Handled = true;
            _didMouseDown = true;
            VisualStateManager.GoToState(this, "Pressed", false);
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

