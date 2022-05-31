using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGWLP.BLL.Models
{
    public class BaseEditableModel : BaseModel
    {
        public string Editor { get; set; }
        public DateTime? Edited { get; set; }
    }
}
