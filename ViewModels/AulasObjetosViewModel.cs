using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using ReservasApp.Helpers;
using ReservasApp.Models;

namespace ReservasApp.ViewModels
{
    public class AulasObjetosViewModel : ObservableObject
    {
        private List<Aula> aulas;
        public List<Aula> Aulas
        {
            get => aulas;
            set => SetProperty(ref aulas, value);
        }

        private string filtroNombre;
        public string FiltroNombre
        {
            get => filtroNombre;
            set => SetProperty(ref filtroNombre, value);
        }

        public ICommand BuscarCommand { get; }

        public AulasObjetosViewModel()
        {
            BuscarCommand = new RelayCommand(_ => CargarAulas(FiltroNombre));
            CargarAulas("");
        }

        private void CargarAulas(string filtro)
        {
            var lista = new List<Aula>();
            using (SqlConnection conn = ConexionHelper.ObtenerConexion())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT AulaId, Nombre, Capacidad FROM Aulas WHERE Nombre LIKE @Filtro", conn))
                {
                    cmd.Parameters.AddWithValue("@Filtro", "%" + (filtro ?? "") + "%");
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Aula
                            {
                                AulaId = reader.GetInt32(reader.GetOrdinal("AulaId")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Capacidad = reader.GetInt32(reader.GetOrdinal("Capacidad"))
                            });
                        }
                    }
                }
            }
            Aulas = lista;
        }
    }
}