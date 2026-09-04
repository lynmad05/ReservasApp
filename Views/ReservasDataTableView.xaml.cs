using System.Windows.Controls;
using ReservasApp.ViewModels;

namespace ReservasApp.Views
{
    public partial class ReservasDataTableView : UserControl
    {
        public ReservasDataTableView()
        {
            InitializeComponent();
            DataContext = new ReservasDataTableViewModel();
        }
    }
}