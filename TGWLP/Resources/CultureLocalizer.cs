using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TGWLP
{
    public class CultureLocalizer
    {
        private IStringLocalizer Localizer { get; }
        
        public CultureLocalizer(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemplyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            Localizer = factory.Create("SharedResource", assemplyName.Name);
        }

        public LocalizedString this[string key] => Localizer[key];

        public LocalizedString LocalizedString(string key, params string[] args)
        {
            return args == null ? Localizer[key] : Localizer[key, args];
        }
    }
}
