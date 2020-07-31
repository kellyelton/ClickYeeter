using System;

namespace ClickYeeter9000
{
    public interface ISettings
    {
        string YeetHotKey { get; set; }

        string RecordHotKey { get; set; }

        string Theme { get; set; }

        bool SoundEnabled { get; set; }
    }
}
