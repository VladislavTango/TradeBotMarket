using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TradeBotMarket.ViewModels.Base
{
    public abstract class ViewModel : INotifyPropertyChanged , IDisposable
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void Dispose()
        {
            Dispose(true);
        }

        private bool _disposed;
        protected virtual void Dispose(bool Disposing) 
        {
            if (!Disposing || _disposed) return;
            _disposed = true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? ProperyName = null) 
        {
            if (Equals(field, value)) return true;
            field = value;

            OnPropertyChanged(ProperyName);
            return true;
        }


    }
}
