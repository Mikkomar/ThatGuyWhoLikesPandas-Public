using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGWLP.BLL.Models
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        public string Creator { get; set; }
        public DateTime Created { get; set; }
    }
}
