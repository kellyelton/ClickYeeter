using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClickYeeter9000
{
    public class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify([CallerMemberName]string property = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected bool SetAndNotify<T>(ref T field, T value, [CallerMemberName]string property = null) {
            if (object.Equals(field, value)) return false;

            field = value;

            Notify(property);

            return true;
        }
    }
}
