using ClickYeeter9000.Components.Events;
using Gma.System.MouseKeyHook;
using System;
using System.Windows.Forms;

namespace ClickYeeter9000.Components
{
    public class Recorder : ModelBase, IDisposable
    {
        public Record Record {
            get => _record;
            private set => SetAndNotify(ref _record, value);
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

                        Notify();
                    } else {
                        Stop();

                        Notify();
                    }
                }
            }
        }

        private Record _record;
        private bool _isEnabled;
        private IKeyboardMouseEvents _hook;

        private readonly object _sync = new object();

        public Recorder() {
            Record = new Record();
        }

        public void Start() {
            lock (_sync) {
                if (_isEnabled) return;

                _isEnabled = true;
            }

            Record = new Record();

            _hook = Hook.GlobalEvents();

            _hook.MouseClick += Hook_MouseClick;
            _hook.MouseDoubleClick += Hook_MouseDoubleClick;
        }

        public void Stop() {
            lock (_sync) {
                if (!_isEnabled) return;

                _isEnabled = false;
            }

            _hook.MouseClick -= Hook_MouseClick;

            _hook?.Dispose();
        }

        private void Hook_MouseClick(object sender, MouseEventArgs args) {
            var eve = new MouseClickEvent(args.Button, args.X, args.Y, Record.GetTime());

            Record.Events.Add(eve);
        }

        private void Hook_MouseDoubleClick(object sender, MouseEventArgs args) {
            var eve = new MouseClickEvent(args.Button, args.X, args.Y, Record.GetTime());

            Record.Events.Add(eve);
        }

        #region IDisposable Support
        private bool _isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!_isDisposed) {
                if (disposing) {
                    Stop();
                }

                _isDisposed = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
