using System.Windows.Controls;
using ReservasApp.ViewModels;

namespace ReservasApp.Views
{
    public partial class AulasDataTableView : UserControl
    {
        public AulasDataTableView()
        {
            InitializeComponent();
            DataContext = new AulasDataTableViewModel();
        }
    }
}