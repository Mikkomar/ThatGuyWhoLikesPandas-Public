using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TGWLP.DAL.Interfaces;

namespace TGWLP.DAL.Repositories
{
    public class RepositoryBase : IRepository
    {
        protected readonly string _connectionString;

        public RepositoryBase(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<DataTable> ExecuteDataTableQueryAsync(string queryString, IEnumerable<SqlParameter> parameters = null) {
            DataTable resultTable = new DataTable();
            
            using (var connection = new SqlConnection(_connectionString)) {
                using (var command = connection.CreateCommand()) {

                    command.CommandText = queryString;

                    if (parameters != null) {
                        foreach (var parameter in parameters) {
                            command.Parameters.Add(parameter);
                        }
                    }

                    connection.Open();
                    resultTable.Load(await command.ExecuteReaderAsync());
                    connection.Close();

                    return resultTable;
                }
            }
        }
    }
}
