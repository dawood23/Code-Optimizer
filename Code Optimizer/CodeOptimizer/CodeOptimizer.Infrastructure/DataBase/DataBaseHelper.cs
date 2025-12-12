using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CodeOptimizer.Infrastructure.Database
{
    public class DataBaseConnection
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DataBaseConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection not configured");
        }

        public SqlConnection GetConnection() => new SqlConnection(_connectionString);

        public SqlCommand CreateCommand(SqlConnection connection, string storedProcedure, Dictionary<string, object>? parameters = null)
        {
            var cmd = new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
                foreach (var p in parameters)
                    cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);

            return cmd;
        }
    }
}
