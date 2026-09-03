using System.Data;
using Microsoft.Data.SqlClient;
using ReservasApp.Helpers;

namespace ReservasApp.ViewModels
{
    public class ReservasDataTableViewModel : ObservableObject
    {
        private DataView reservas;
        public DataView Reservas
        {
            get => reservas;
            set => SetProperty(ref reservas, value);
        }

        public ReservasDataTableViewModel()
        {
            CargarReservas();
        }

        private void CargarReservas()
        {
            DataTable tabla = new DataTable();
            using (SqlConnection conn = ConexionHelper.ObtenerConexion())
            {
                string query = @"
                    SELECT r.ReservaId, a.Nombre AS Aula, u.NombreCompleto AS Usuario,
                           r.Fecha, r.Hora, r.Motivo
                    FROM Reservas r
                    INNER JOIN Aulas a ON r.AulaId = a.AulaId
                    INNER JOIN Usuarios u ON r.UsuarioId = u.UsuarioId";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.Fill(tabla);
            }
            Reservas = tabla.DefaultView;
        }
    }
}