using System;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using ReservasApp.Helpers;
using ReservasApp.Models;

namespace ReservasApp.ViewModels
{
    public class ReservasObjetosViewModel : ObservableObject
    {
        private List<Reserva> reservas;
        public List<Reserva> Reservas
        {
            get => reservas;
            set => SetProperty(ref reservas, value);
        }

        private DateTime? fechaFiltro;
        public DateTime? FechaFiltro
        {
            get => fechaFiltro;
            set => SetProperty(ref fechaFiltro, value);
        }

        public ICommand BuscarCommand { get; }
        public ICommand VerTodasCommand { get; }

        public ReservasObjetosViewModel()
        {
            BuscarCommand = new RelayCommand(_ => CargarReservas(FechaFiltro));
            VerTodasCommand = new RelayCommand(_ => CargarReservas(null));
            CargarReservas(null);
        }

        private void CargarReservas(DateTime? fecha)
        {
            var lista = new List<Reserva>();
            using (SqlConnection conn = ConexionHelper.ObtenerConexion())
            {
                conn.Open();
                string query = @"
                    SELECT r.ReservaId, r.AulaId, r.UsuarioId,
                           a.Nombre AS NombreAula, u.NombreCompleto AS NombreUsuario,
                           r.Fecha, r.Hora, r.Motivo
                    FROM Reservas r
                    INNER JOIN Aulas a ON r.AulaId = a.AulaId
                    INNER JOIN Usuarios u ON r.UsuarioId = u.UsuarioId";
                if (fecha.HasValue) query += " WHERE r.Fecha = @Fecha";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (fecha.HasValue) cmd.Parameters.AddWithValue("@Fecha", fecha.Value.Date);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Reserva
                            {
                                ReservaId = reader.GetInt32(reader.GetOrdinal("ReservaId")),
                                AulaId = reader.GetInt32(reader.GetOrdinal("AulaId")),
                                UsuarioId = reader.GetInt32(reader.GetOrdinal("UsuarioId")),
                                NombreAula = reader.GetString(reader.GetOrdinal("NombreAula")),
                                NombreUsuario = reader.GetString(reader.GetOrdinal("NombreUsuario")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                Hora = reader.GetTimeSpan(reader.GetOrdinal("Hora")),
                                Motivo = reader.IsDBNull(reader.GetOrdinal("Motivo")) ? "" : reader.GetString(reader.GetOrdinal("Motivo"))
                            });
                        }
                    }
                }
            }
            Reservas = lista;
        }
    }
}