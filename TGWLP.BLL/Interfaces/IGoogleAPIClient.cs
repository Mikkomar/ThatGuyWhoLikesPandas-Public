using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGWLP.BLL.Interfaces
{
    public interface IGoogleAPIClient
    {
        public Task<string> GetBookByIdAsync(string bookId);
    }
}
