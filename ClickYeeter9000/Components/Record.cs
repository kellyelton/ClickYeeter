using System;
using System.Collections.ObjectModel;

namespace ClickYeeter9000.Components
{
    public class Record : ModelBase
    {
        public DateTime StartTime {
            get => _startTime;
            set => SetAndNotify(ref _startTime, value);
        }

        private DateTime _startTime;

        public ObservableCollection<IEvent> Events {
            get => _events;
            set => SetAndNotify(ref _events, value);
        }

        private ObservableCollection<IEvent> _events;

        public Record() {
            _events = new ObservableCollection<IEvent>();

            _startTime = DateTime.Now;
        }

        public int GetTime() {
            // This conversion may cause weird issues, keep this in mind
            return (int)(DateTime.Now - _startTime).TotalMilliseconds;
        }
    }
}
