using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TGWLP.BLL;

namespace TGWLP.BLL.Clients
{
    public abstract class ClientBase
    {
        protected IHttpClientFactory _httpClientFactory { get; }
        protected IOptions<AppSettings> _settings { get; }

        public ClientBase(IHttpClientFactory httpClientFactory, IOptions<AppSettings> settings)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings;
        }
    }
}
