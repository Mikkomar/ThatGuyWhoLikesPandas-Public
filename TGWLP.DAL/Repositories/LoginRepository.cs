using TGWLP.DAL.Entities;
using TGWLP.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TGWLP.DAL.Repositories
{
    public class LoginRepository : RepositoryBase
    {
        private IConfiguration Configuration { get; }
        private IAppContext Context { get; }
        private int TimeBetweenLoginsMinutes { get; set; }
        private int MaxAllowedLoginAttempts { get; set; }

        public LoginRepository(IAppContext context, IConfiguration config) : base(config)
        {
            this.Configuration = config;
            this.Context = context;

            this.MaxAllowedLoginAttempts = Int32.Parse(this.Configuration.GetSection("LoginSettings")["MaxAttemptedLogins"]);
            this.TimeBetweenLoginsMinutes = Int32.Parse(this.Configuration.GetSection("LoginSettings")["TimeBetweenLoginsMinutes"]);
        }

        /// <summary>
        /// Creates a record of a login attempt.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="time"></param>
        /// <param name="IP"></param>
        /// <param name="successful"></param>
        public void LogLoginAttempt(string username, DateTime time, string IP, bool successful)
        {
            using (SqlConnection connection = new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection")))
            {

                string sqlString = $@"INSERT INTO Log_Login (Username, Time, IP, Successful) VALUES(@username, @time, @ip, @successful);";

                SqlCommand command = new SqlCommand() {
                    CommandText = sqlString,
                    CommandType = CommandType.Text,
                    Connection = connection
                };

                command.Parameters.Add(new SqlParameter("@username", username));
                command.Parameters.Add(new SqlParameter("@time", time));
                command.Parameters.Add(new SqlParameter("@ip", IP));
                command.Parameters.Add(new SqlParameter("@successful", successful));

                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Checks if visitor with IP address has tried logging in too many times.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="IP"></param>
        /// <returns></returns>
        public bool CanTryLogin(DateTime time, string IP)
        {
            using (SqlConnection connection = new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection")))
            {
                DataSet myDS = new DataSet();
                string sqlString = $@"  SELECT CASE WHEN (SELECT COUNT(*) FROM Log_Login WHERE IP = @ip) < {MaxAllowedLoginAttempts} THEN CAST(1 AS BIT)
                                        WHEN ISNULL(DATEDIFF(MINUTE, (SELECT TOP 1 Amount FROM
	                                            (SELECT TOP {MaxAllowedLoginAttempts} Time AS Amount FROM Log_Login WHERE IP = @ip ORDER BY Amount DESC) AS Amounts ORDER BY Amount ASC), 
                                            @time), 0) > {TimeBetweenLoginsMinutes} 
                                        THEN CAST(1 AS BIT) 
                                        ELSE CAST(0 AS BIT) END AS CanTryLogin";

                SqlCommand command = new SqlCommand()
                {
                    CommandText = sqlString,
                    CommandType = CommandType.Text,
                    Connection = connection
                };

                command.Parameters.Add(new SqlParameter("@time", time));
                command.Parameters.Add(new SqlParameter("@ip", IP));

                connection.Open();

                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = command;
                    adapter.Fill(myDS);
                }

                return (bool)myDS.Tables[0].Rows[0]["CanTryLogin"];
            }
        }
    }
}
