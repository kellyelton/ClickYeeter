using System;
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
    }
}