using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGWLP.DAL.Interfaces
{
    public interface IEditable
    {
        public Guid? Editor { get; set; }
        public DateTime? Edited { get; set; }
    }
}
