using System.Windows.Controls;
using ReservasApp.ViewModels;

namespace ReservasApp.Views
{
    public partial class AulasObjetosView : UserControl
    {
        public AulasObjetosView()
        {
            InitializeComponent();
            DataContext = new AulasObjetosViewModel();
        }
    }
}