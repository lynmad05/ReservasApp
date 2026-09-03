using System;
using System.Collections.Generic;
using System.Text;

using System.Configuration;
using Microsoft.Data.SqlClient;

namespace ReservasApp.Helpers
{
    public static class ConexionHelper
    {
        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["ReservasDB"].ConnectionString;

        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(connectionString);
        }
    }
}
