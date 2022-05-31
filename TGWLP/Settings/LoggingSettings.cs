using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGWLP.Settings
{
    public class LoggingSettings
    {
        public LogLevelSettings LogLevel { get; set; }
    }

    public class LogLevelSettings
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }
        public string Lifetime { get; set; }
    }
}
