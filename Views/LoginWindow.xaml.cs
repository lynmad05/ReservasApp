using System.Windows;
using Microsoft.Data.SqlClient;
using ReservasApp.Helpers;
using ReservasApp.Views;

namespace ReservasApp.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsername.Text.Trim();
            string clave = txtPassword.Password;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(clave))
            {
                txtMensaje.Text = "Ingresa usuario y contraseña.";
                return;
            }

            using (SqlConnection conn = ConexionHelper.ObtenerConexion())
            {
                conn.Open();

                string query = "SELECT UsuarioId, NombreCompleto FROM Usuarios " +
               "WHERE Username = @Username AND Password = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", usuario);
                    cmd.Parameters.AddWithValue("@Password", clave);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // hay una fila -> credenciales correctas
                        {
                            SesionActual.UsuarioId = reader.GetInt32(reader.GetOrdinal("UsuarioId"));
                            SesionActual.NombreCompleto = reader.GetString(reader.GetOrdinal("NombreCompleto"));

                            MessageBox.Show($"Bienvenido, {SesionActual.NombreCompleto}!", "Acceso concedido",
                                            MessageBoxButton.OK, MessageBoxImage.Information);

                            MenuPrincipalWindow menu = new MenuPrincipalWindow();
                            menu.Show();
                            this.Close();
                        }
                        else
                        {
                            txtMensaje.Text = "Usuario o contraseña incorrectos.";
                        }
                    }
                }
            } 
        }
    }
}