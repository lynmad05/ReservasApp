using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;
using ReservasApp.Helpers;
using ReservasApp.ViewModels;

namespace ReservasApp.Views
{
    public partial class AulasDataTableView : Window
    {
        public AulasDataTableView()
        {
            InitializeComponent();
            DataContext = new AulasDataTableViewModel();
        }
    }
}