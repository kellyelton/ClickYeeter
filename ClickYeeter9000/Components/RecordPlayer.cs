using System;
using System.Threading;

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
        private readonly Timer _clickTimer;

        public RecordPlayer() {
            _clickTimer = new Timer(OnTick, this, Timeout.Infinite, Timeout.Infinite);
        }

        private void Start() {
            lock (_sync) {
                if (_isStarted) return;

                _isStarted = true;

                if (!_isTicking) {
                    _lastEvent = -1; // reset playback
                    _startTime = DateTime.Now;

                    QueueNextTick();
                }
            }
        }

        private void Stop() {
            lock (_sync) {
                if (!_isStarted) return;

                _isStarted = false;
            }
        }

        Record _previousRecord;
        DateTime _startTime;
        int _lastEvent = -1;
        bool _isStarted;
        bool _isTicking;


        private void OnTick(object state) {
            lock (_sync) {
                if (!_isStarted) return;
                _isTicking = true;
            }

            try {

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
            } finally {
                lock (_sync) {
                    _isTicking = false;

                    if (_isStarted) {
                        QueueNextTick();
                    }
                }
            }
        }

        private void QueueNextTick() {
            lock (_sync) {
                _clickTimer.Change(25, Timeout.Infinite);
            }
        }
    }
}
