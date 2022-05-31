using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TGWLP.DAL.Interfaces
{
    public interface IRepository
    {
        public Task<DataTable> ExecuteDataTableQueryAsync(string queryString, IEnumerable<SqlParameter> parameters);
    }
}
