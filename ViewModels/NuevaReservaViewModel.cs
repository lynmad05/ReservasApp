using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using ReservasApp.Helpers;
using ReservasApp.Models;

namespace ReservasApp.ViewModels
{
    public class NuevaReservaViewModel : ObservableObject
    {
        private readonly int usuarioIdActual;
        public List<Aula> Aulas { get; }

        private Aula aulaSeleccionada;
        public Aula AulaSeleccionada
        {
            get => aulaSeleccionada;
            set => SetProperty(ref aulaSeleccionada, value);
        }

        private DateTime? fecha;
        public DateTime? Fecha
        {
            get => fecha;
            set => SetProperty(ref fecha, value);
        }

        private string horaTexto = "08:00";
        public string HoraTexto
        {
            get => horaTexto;
            set => SetProperty(ref horaTexto, value);
        }

        private string motivo;
        public string Motivo
        {
            get => motivo;
            set => SetProperty(ref motivo, value);
        }

        public ICommand GuardarCommand { get; }

        // El code-behind se suscribe a esto para cerrar la ventana
        public event Action SolicitaCierre;

        public NuevaReservaViewModel(int usuarioId)
        {
            usuarioIdActual = usuarioId;
            Aulas = CargarAulas();
            GuardarCommand = new RelayCommand(_ => Guardar());
        }

        private List<Aula> CargarAulas()
        {
            var lista = new List<Aula>();
            using (SqlConnection conn = ConexionHelper.ObtenerConexion())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT AulaId, Nombre, Capacidad FROM Aulas", conn))
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
            return lista;
        }

        private void Guardar()
        {
            if (AulaSeleccionada == null) { MessageBox.Show("Selecciona un aula."); return; }
            if (!Fecha.HasValue) { MessageBox.Show("Selecciona una fecha."); return; }
            if (!TimeSpan.TryParse(HoraTexto?.Trim(), out TimeSpan hora))
            {
                MessageBox.Show("Hora inválida. Usa el formato HH:mm.");
                return;
            }

            using (SqlConnection conn = ConexionHelper.ObtenerConexion())
            {
                conn.Open();

                using (SqlCommand cmdVerificar = new SqlCommand(
                    "SELECT COUNT(*) FROM Reservas WHERE AulaId=@AulaId AND Fecha=@Fecha AND Hora=@Hora", conn))
                {
                    cmdVerificar.Parameters.AddWithValue("@AulaId", AulaSeleccionada.AulaId);
                    cmdVerificar.Parameters.AddWithValue("@Fecha", Fecha.Value.Date);
                    cmdVerificar.Parameters.AddWithValue("@Hora", hora);

                    if ((int)cmdVerificar.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("Ya existe una reserva para esa aula, fecha y hora.",
                                        "Reserva duplicada", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                using (SqlCommand cmdInsertar = new SqlCommand(
                    "INSERT INTO Reservas (AulaId, UsuarioId, Fecha, Hora, Motivo) VALUES (@AulaId, @UsuarioId, @Fecha, @Hora, @Motivo)", conn))
                {
                    cmdInsertar.Parameters.AddWithValue("@AulaId", AulaSeleccionada.AulaId);
                    cmdInsertar.Parameters.AddWithValue("@UsuarioId", usuarioIdActual);
                    cmdInsertar.Parameters.AddWithValue("@Fecha", Fecha.Value.Date);
                    cmdInsertar.Parameters.AddWithValue("@Hora", hora);
                    cmdInsertar.Parameters.AddWithValue("@Motivo", string.IsNullOrEmpty(Motivo) ? (object)DBNull.Value : Motivo);
                    cmdInsertar.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Reserva registrada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            SolicitaCierre?.Invoke();
        }
    }
}