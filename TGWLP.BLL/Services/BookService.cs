using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGWLP.BLL.Interfaces;
using TGWLP.BLL.Models;
using Newtonsoft.Json;

namespace TGWLP.BLL.Services
{
    public class BookService : IBookService
    {
        public IGoogleAPIClient _googleAPIClient { get; }

        public BookService(IGoogleAPIClient googleAPIClient)
        {
            _googleAPIClient = googleAPIClient;
        }

        public async Task<GoogleBooksVolumeModel> GetBookInformationByIdAsync(string bookId)
        {
            var resultJson = await _googleAPIClient.GetBookByIdAsync(bookId);
            return JsonConvert.DeserializeObject<GoogleBooksVolumeModel>(resultJson);
        }
    }
}
