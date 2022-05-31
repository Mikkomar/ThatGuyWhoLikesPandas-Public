using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TGWLP.DAL.Interfaces;
using TGWLP.DAL.Entities;
using TGWLP.DAL.Repositories;

namespace TGWLP.DAL.Repositories
{
    public class StoryRepository : RepositoryBase
    {
        public StoryRepository(IConfiguration configuration) : base(configuration) {
        }

        public async Task<DataTable> GetPublishedStoriesAsync() {
            var today = DateTime.Now.Date;

            string queryString = @"
            SELECT Id, Title, Text, PublishDate
            FROM Story
            WHERE ISNULL(PublishDate, '9999-12-31') <= @today
            ";

            List<SqlParameter> parameters = new List<SqlParameter>() {
                new SqlParameter("@today", today)
            };

            return await ExecuteDataTableQueryAsync(queryString, parameters);
        }

        public async Task<DataTable> GetPublishedStoriesByKeywordAsync(string keyword) {
            var today = DateTime.Now.Date;

            string queryString = @"
            SELECT Id, Title, Text, PublishDate
            FROM Story
            WHERE Title LIKE @keyword AND Text LIKE @keyword
            AND ISNULL(PublishDate, '9999-12-31') <= @today
            UNION
            SELECT Id, Title, Text, PublishDate
            FROM Story
            WHERE (Title LIKE @keyword OR Text LIKE @keyword)
            AND ISNULL(PublishDate, '9999-12-31') <= @today
            ";

            List<SqlParameter> parameters = new List<SqlParameter>() {
                new SqlParameter("@today", today),
                new SqlParameter("@keyword", '%' + keyword + '%')
            };

            return await ExecuteDataTableQueryAsync(queryString, parameters);
        }
    }
}
