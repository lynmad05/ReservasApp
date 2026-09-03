using System.Data;
using Microsoft.Data.SqlClient;
using ReservasApp.Helpers;

namespace ReservasApp.ViewModels
{
    public class AulasDataTableViewModel : ObservableObject
    {
        private DataView aulas;
        public DataView Aulas
        {
            get => aulas;
            set => SetProperty(ref aulas, value);
        }

        public AulasDataTableViewModel()
        {
            CargarAulas();
        }

        private void CargarAulas()
        {
            DataTable tabla = new DataTable();
            using (SqlConnection conn = ConexionHelper.ObtenerConexion())
            {
                SqlDataAdapter adapter = new SqlDataAdapter(
                    "SELECT AulaId, Nombre, Capacidad FROM Aulas", conn);
                adapter.Fill(tabla);
            }
            Aulas = tabla.DefaultView;
        }
    }
}