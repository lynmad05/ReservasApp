using System;
using System.Windows;
using Microsoft.Data.SqlClient;
using ReservasApp.Helpers;
using ReservasApp.Models;
using ReservasApp.ViewModels;

namespace ReservasApp.Views
{
    public partial class NuevaReservaView : Window
    {

        private int usuarioIdActual;

        public NuevaReservaView(int usuarioId)
        {
            InitializeComponent();
            var vm = new NuevaReservaViewModel(usuarioId);
            vm.SolicitaCierre += () => this.Close();
            DataContext = vm;
        }

    }
}