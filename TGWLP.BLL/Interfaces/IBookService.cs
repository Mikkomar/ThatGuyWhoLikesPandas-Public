using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGWLP.BLL.Models;

namespace TGWLP.BLL.Interfaces
{
    public interface IBookService
    {
        public Task<GoogleBooksVolumeModel> GetBookInformationByIdAsync(string bookId);
    }
}
