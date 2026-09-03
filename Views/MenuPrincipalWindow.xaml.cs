using System.Windows;
using ReservasApp.Helpers;

namespace ReservasApp.Views
{
    public partial class MenuPrincipalWindow : Window
    {
        public MenuPrincipalWindow()
        {
            InitializeComponent();
            txtBienvenida.Text = $"Hola, {SesionActual.NombreCompleto}";
        }

        private void btnAulasDataTable_Click(object sender, RoutedEventArgs e)
        {
            new AulasDataTableView().Show();
        }

        private void btnAulasObjetos_Click(object sender, RoutedEventArgs e)
        {
            new AulasObjetosView().Show();
        }

        private void btnReservasDataTable_Click(object sender, RoutedEventArgs e)
        {
            new ReservasDataTableView().Show();
        }

        private void btnReservasObjetos_Click(object sender, RoutedEventArgs e)
        {
            new ReservasObjetosView().Show();
        }

        private void btnNuevaReserva_Click(object sender, RoutedEventArgs e)
        {
            new NuevaReservaView(SesionActual.UsuarioId).Show();
        }
    }
}