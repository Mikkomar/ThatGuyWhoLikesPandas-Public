using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGWLP.BLL.Settings
{
    public class LoginSettings
    {
        public int MaxAttemptedLogins { get; set; }
        public int TimeBetweenLoginsMinutes { get; set; }
    }
}
