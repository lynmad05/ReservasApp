using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using ReservasApp.Helpers;

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
                MostrarError("Ingresa usuario y contraseña.");
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
                        if (reader.Read())
                        {
                            SesionActual.UsuarioId = reader.GetInt32(reader.GetOrdinal("UsuarioId"));
                            SesionActual.NombreCompleto = reader.GetString(reader.GetOrdinal("NombreCompleto"));

                            MenuPrincipalWindow menu = new MenuPrincipalWindow();
                            menu.Show();
                            this.Close();
                        }
                        else
                        {
                            MostrarError("Usuario o contraseña incorrectos.");
                        }
                    }
                }
            }
        }

        private void MostrarError(string mensaje)
        {
            txtMensaje.Text = mensaje;
            txtMensaje.Visibility = Visibility.Visible;
        }
    }
}