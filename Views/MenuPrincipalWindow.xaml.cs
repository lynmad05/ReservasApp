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

        private void Sidebar_NavigationRequested(object sender, string vista)
        {
            if (vista == "NuevaReserva")
            {
                var nuevaReserva = new NuevaReservaView(SesionActual.UsuarioId);
                nuevaReserva.Owner = this;
                nuevaReserva.ShowDialog();
                return;
            }

            Sidebar.ActiveView = vista;

            ContentArea.Content = vista switch
            {
                "AulasDataTable" => new AulasDataTableView(),
                "AulasObjetos" => new AulasObjetosView(),
                "ReservasDataTable" => new ReservasDataTableView(),
                "ReservasObjetos" => new ReservasObjetosView(),
                _ => ContentArea.Content
            };
        }
    }
}