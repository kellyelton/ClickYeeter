using System;
using System.Windows.Forms;

namespace ClickYeeter9000.Components.Events
{
    public class MouseClickEvent : IEvent
    {
        public MouseButtons Button { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Time { get; set; }

        public MouseClickEvent() { }

        public MouseClickEvent(MouseButtons button, int x, int y, int time) {
            Button = button;
            X = x;
            Y = y;
            Time = time;
        }
    }
}
