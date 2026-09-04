using System.Windows.Controls;
using ReservasApp.ViewModels;

namespace ReservasApp.Views
{
    public partial class ReservasObjetosView : UserControl
    {
        public ReservasObjetosView()
        {
            InitializeComponent();
            DataContext = new ReservasObjetosViewModel();
        }
    }
}