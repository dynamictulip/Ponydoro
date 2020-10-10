using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Ponydoro_WPF.Mvvm
{
    [ExcludeFromCodeCoverage]
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}