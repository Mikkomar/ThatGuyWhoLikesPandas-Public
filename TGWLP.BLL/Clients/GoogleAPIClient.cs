using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TGWLP.BLL.Interfaces;

namespace TGWLP.BLL.Clients
{
    public class GoogleAPIClient : ClientBase, IGoogleAPIClient
    {
        public GoogleAPIClient(IHttpClientFactory httpClientFactory, IOptions<AppSettings> settings) : base(httpClientFactory, settings)
        {
        }

        public async Task<string> GetBookByIdAsync(string bookId)
        {
            using (var client = _httpClientFactory.CreateClient("GoogleBooks"))
            {
                var endpoint = $"volumes/{bookId}";
                return await client.GetStringAsync(endpoint);
            }
        }
    }
}
