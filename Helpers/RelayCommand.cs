using System;
using System.Windows.Input;

namespace ReservasApp.Helpers
{
  
    public class RelayCommand : ICommand
    {
        private readonly Action<object> ejecutar;

        public RelayCommand(Action<object> ejecutar)
        {
            this.ejecutar = ejecutar;
        }

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => ejecutar(parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}