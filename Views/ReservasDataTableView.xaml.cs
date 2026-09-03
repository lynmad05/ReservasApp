using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;
using ReservasApp.Helpers;
using ReservasApp.ViewModels;

namespace ReservasApp.Views
{
    public partial class ReservasDataTableView : Window
    {
        public ReservasDataTableView()
        {
            InitializeComponent();
            DataContext = new ReservasDataTableViewModel();
        }
    }
}