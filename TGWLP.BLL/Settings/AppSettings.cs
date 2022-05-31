using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TGWLP.BLL.Settings;

namespace TGWLP.BLL
{
    public class AppSettings
    {
        public AzureAdSettings AzureAd { get; set; }
        public PasswordSettings PasswordSettings { get; set; }
        public LogLevelSettings LogLevel { get; set; }
        public LoginSettings LoginSettings { get; set; }
        public ConnectionStringSettings ConnectionStrings { get; set; }
        public DefaultValueSettings DefaultValues { get; set; }
        public string AllowedHosts { get; set; }
        public APISettings API { get; set; }
    }
}
