using System;
using System.Threading;
using System.Windows.Forms;

namespace ClickYeeter9000.Components
{
    internal class MouseDoubleClickEvent : IEvent
    {
        public int Time { get; set; }

        public MouseButtons Button { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public MouseDoubleClickEvent() { }

        public MouseDoubleClickEvent(MouseButtons button, int x, int y, int time) {
            Button = button;
            X = x;
            Y = y;
            Time = time;
        }

        public void Run() {
            Mouse.SetCursorPosition(X, Y);

            if (Button == MouseButtons.Left) {
                Mouse.MouseEvent(Mouse.MouseEventFlags.LeftDown);

                Mouse.MouseEvent(Mouse.MouseEventFlags.LeftUp);
            } else if(Button == MouseButtons.Right) {
                Mouse.MouseEvent(Mouse.MouseEventFlags.RightDown);

                Mouse.MouseEvent(Mouse.MouseEventFlags.RightUp);
            } else if(Button == MouseButtons.Middle) {
                Mouse.MouseEvent(Mouse.MouseEventFlags.MiddleDown);

                Mouse.MouseEvent(Mouse.MouseEventFlags.MiddleUp);
            }

            Thread.Sleep(5);

            if (Button == MouseButtons.Left) {
                Mouse.MouseEvent(Mouse.MouseEventFlags.LeftDown);

                Mouse.MouseEvent(Mouse.MouseEventFlags.LeftUp);
            } else if(Button == MouseButtons.Right) {
                Mouse.MouseEvent(Mouse.MouseEventFlags.RightDown);

                Mouse.MouseEvent(Mouse.MouseEventFlags.RightUp);
            } else if(Button == MouseButtons.Middle) {
                Mouse.MouseEvent(Mouse.MouseEventFlags.MiddleDown);

                Mouse.MouseEvent(Mouse.MouseEventFlags.MiddleUp);
            }
        }
    }
}