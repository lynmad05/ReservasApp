using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Data.SqlClient;
using ReservasApp.Helpers;
using ReservasApp.Models;
using ReservasApp.ViewModels;

namespace ReservasApp.Views
{
    public partial class ReservasObjetosView : Window
    {
        public ReservasObjetosView()
        {
            InitializeComponent();
            DataContext = new ReservasObjetosViewModel();
        }


    }
}