using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ReservasApp.Helpers
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        protected bool SetProperty<T>(ref T campo, T valor, [CallerMemberName] string propName = null)
        {
            if (Equals(campo, valor)) return false;
            campo = valor;
            OnPropertyChanged(propName);
            return true;
        }
    }
}