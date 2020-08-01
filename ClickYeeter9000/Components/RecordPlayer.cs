using System;
using System.Diagnostics;
using System.Timers;

namespace ClickYeeter9000.Components
{
    public class RecordPlayer : ModelBase
    {
        public Record Record {
            get => _record;
            set => SetAndNotify(ref _record, value);
        }

        public bool IsEnabled {
            get {
                lock (_sync) {
                    return _isEnabled;
                }
            }
            set {
                lock (_sync) {
                    if (value == _isEnabled) return;

                    if (value) {
                        Start();

                        _isEnabled = true;

                        Notify();
                    } else {
                        Stop();

                        _isEnabled = false;

                        Notify();
                    }
                }
            }
        }

        private Record _record;
        private bool _isEnabled;

        private readonly object _sync = new object();
        private readonly Timer _clickTimer = new Timer(5);

        public RecordPlayer() {
            _clickTimer.Elapsed += ClickTimerElapsed;
        }

        private void Start() {
            _clickTimer.Enabled = true;
        }

        private void Stop() {
            _clickTimer.Enabled = false;
        }

        Record _previousRecord;
        DateTime _startTime;
        int _lastEvent = -1;
        bool _inClick;

        private void ClickTimerElapsed(object sender, ElapsedEventArgs e) {
            lock (_sync) {
                _clickTimer.Enabled = false;

                if (_inClick) return;

                _inClick = true;
            }

            var record = Record;

            if (record == null) {
                _previousRecord = null;

                return;
            }

            if (record != _previousRecord) {
                _previousRecord = record;

                // start over
                _lastEvent = -1;
                _startTime = DateTime.Now;
            }

            var startIndex = _lastEvent + 1;

            if (startIndex >= record.Events.Count) {
                startIndex = 0;

                // start over
                _lastEvent = -1;
                _startTime = DateTime.Now;
            }

            for (var i = startIndex; i < record.Events.Count; i++) {
                var eve = record.Events[i];

                var elapsed = (DateTime.Now - _startTime).TotalMilliseconds;

                if (elapsed < eve.Time) break;

                eve.Run();

                _lastEvent = i;
            }

            lock (_sync) {
                _inClick = false;

                _clickTimer.Enabled = true;
            }
        }
    }
}
