using Microsoft.EntityFrameworkCore.Query;
using System.Data.SqlClient;
using Npgsql;

namespace BasicAuth.Helper
{
    public class CommonHelper
    {
        private IConfiguration _config;

        public CommonHelper (IConfiguration config)
        {
            _config = config;
        }

        public int DMLTransaction(string query)
        {
            int result = 0;
            string connectionString = _config["ConnectionStrings:DefaultConnection"];

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    result = command.ExecuteNonQuery();
                }
            }

            return result;
        }

        public bool UserAlreadyExists(string query)
        {
            bool flag = false;
            string connectionString = _config["ConnectionStrings:DefaultConnection"];

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    using (NpgsqlDataReader rd = command.ExecuteReader())
                    {
                        if (rd.HasRows)
                        {
                            flag = true;
                        }
                    }
                }
            }

            return flag;
        }

    }
}
