using CodeOptimizer.Domain.Entities;
using CodeOptimizer.Domain.Models;
using CodeOptimizer.Infrastructure.Database;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Repositories.Metrices
{
    public class MetricesRepository : IMetricesRepository
    {
        private readonly DataBaseConnection _db;

        public MetricesRepository(DataBaseConnection db)
        {
            _db = db;
        }

        public async Task AddMetric(AddMetricRequest addMetricRequest)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = _db.CreateCommand(conn, "sp_AddMetric", new Dictionary<string, object>
            {
                { "@MetricName", addMetricRequest.MetricName },
                { "@LanguageName", addMetricRequest.Language },
                { "@TimeTaken", addMetricRequest.TimeTaken }
            });

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<Entities.OptimizationCallMetric>?> GetOptimizationCallsByLanguage()
        {
            var list = new List<Entities.OptimizationCallMetric>();

            using var conn = _db.GetConnection();
            conn.Open();

            using var command = _db.CreateCommand(conn, "sp_GetOptimizationCallsByLanguage");
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Entities.OptimizationCallMetric
                {
                    Language = reader["LanguageName"]?.ToString() ?? "",
                    LanguageCallCount =
                        reader["LanguageCallCount"] != DBNull.Value
                            ? Convert.ToInt32(reader["LanguageCallCount"])
                            : 0
                });
            }

            return list;
        }

        public async Task<List<Entities.OptimizationCallTimeMetric>?> GetOptimizationTimeByLanguage()
        {
            var list = new List<Entities.OptimizationCallTimeMetric>();

            using var conn = _db.GetConnection();
            conn.Open();

            using var command = _db.CreateCommand(conn, "sp_GetOptimizationTimeByLanguage");
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Entities.OptimizationCallTimeMetric
                {
                    Language = reader["LanguageName"]?.ToString() ?? "",
                    AvgTimeTaken = reader.IsDBNull(reader.GetOrdinal("AvgTimeTaken"))
                        ? 0m
                        : Convert.ToDecimal(reader["AvgTimeTaken"])
                });
            }

            return list;
        }

        public async Task<Entities.NumberOfUsersMetric> GetTotalUsers()
        {
            using var conn = _db.GetConnection();
            conn.Open();

            using var command = _db.CreateCommand(conn, "sp_GetTotalUsers");
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Entities.NumberOfUsersMetric
                {
                    UserCount = int.TryParse(reader["UserCount"]?.ToString(), out int val)
                        ? val
                        : 0
                };
            }

            return new Entities.NumberOfUsersMetric { UserCount = 0 };
        }

        public async Task<Entities.NumberOfUsersMetric> GetUserCountLastMonth()
        {
            using var conn = _db.GetConnection();
            conn.Open();

            using var command = _db.CreateCommand(conn, "sp_GetUsersFromLastMonth");
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Entities.NumberOfUsersMetric
                {
                    UserCount = int.TryParse(reader["UserCount"]?.ToString(), out int val)
                        ? val
                        : 0
                };
            }

            return new Entities.NumberOfUsersMetric { UserCount = 0 };
        }

        public async Task<List<Entities.UserPerMonthCurrentYear>> GetUsersPerMonth()
        {
            var list = new List<Entities.UserPerMonthCurrentYear>();

            using var conn = _db.GetConnection();
            conn.Open();

            using var command = _db.CreateCommand(conn, "sp_GetUsersByMonth");
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Entities.UserPerMonthCurrentYear
                {
                    Month = reader["MonthName"]?.ToString() ?? "",
                    User = reader.IsDBNull(reader.GetOrdinal("UserCount"))
                        ? 0
                        : Convert.ToInt32(reader["UserCount"])
                });
            }

            return list;
        }
    }
}
