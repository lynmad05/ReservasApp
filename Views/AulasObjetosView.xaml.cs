using System.Collections.Generic;
using System.Windows;
using Microsoft.Data.SqlClient;
using ReservasApp.Helpers;
using ReservasApp.Models;
using ReservasApp.ViewModels;

namespace ReservasApp.Views
{
    public partial class AulasObjetosView : Window
    {
        public AulasObjetosView()
        {
            InitializeComponent();
            DataContext = new AulasObjetosViewModel();
        }
    }
}