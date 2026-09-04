using System;
using System.Windows;
using System.Windows.Controls;
using ReservasApp.Helpers;

namespace ReservasApp.Views
{
    public partial class SidebarMenu : UserControl
    {
        public event EventHandler<string> NavigationRequested;

        public SidebarMenu()
        {
            InitializeComponent();
            txtUsuario.Text = $"👤 {SesionActual.NombreCompleto}";
        }

        public static readonly DependencyProperty ActiveViewProperty =
            DependencyProperty.Register(nameof(ActiveView), typeof(string), typeof(SidebarMenu),
                new PropertyMetadata("", OnActiveViewChanged));

        public string ActiveView
        {
            get => (string)GetValue(ActiveViewProperty);
            set => SetValue(ActiveViewProperty, value);
        }

        private static void OnActiveViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SidebarMenu sidebar)
                sidebar.ResaltarBotonActivo((string)e.NewValue);
        }

        private void ResaltarBotonActivo(string activeView)
        {
            var normal = (Style)FindResource("SidebarButton");
            var activo = (Style)FindResource("SidebarButtonActive");

            btnAulasDataTable.Style = activeView == "AulasDataTable" ? activo : normal;
            btnAulasObjetos.Style = activeView == "AulasObjetos" ? activo : normal;
            btnReservasDataTable.Style = activeView == "ReservasDataTable" ? activo : normal;
            btnReservasObjetos.Style = activeView == "ReservasObjetos" ? activo : normal;
            btnNuevaReserva.Style = activeView == "NuevaReserva" ? activo : normal;
        }

        private void btnAulasDataTable_Click(object sender, RoutedEventArgs e) => NavigationRequested?.Invoke(this, "AulasDataTable");
        private void btnAulasObjetos_Click(object sender, RoutedEventArgs e) => NavigationRequested?.Invoke(this, "AulasObjetos");
        private void btnReservasDataTable_Click(object sender, RoutedEventArgs e) => NavigationRequested?.Invoke(this, "ReservasDataTable");
        private void btnReservasObjetos_Click(object sender, RoutedEventArgs e) => NavigationRequested?.Invoke(this, "ReservasObjetos");
        private void btnNuevaReserva_Click(object sender, RoutedEventArgs e) => NavigationRequested?.Invoke(this, "NuevaReserva");

        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginWindow();
            login.Show();
            foreach (Window w in Application.Current.Windows)
                if (w != login) w.Close();
        }
    }
}