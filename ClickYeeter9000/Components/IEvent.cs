using System;

namespace ClickYeeter9000.Components
{
    public interface IEvent
    {
        int Time { get; }

        void Run();
    }
}
