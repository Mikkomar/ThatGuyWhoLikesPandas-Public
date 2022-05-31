using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGWLP.Settings
{
    public class PasswordSettings
    {
        public int MinLength { get; set; }
        public bool RequireDigits { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireLowercase { get; set; }
    }
}
