using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TGWLP.DAL.Interfaces;

namespace TGWLP.DAL.Entities
{
    public class BaseEditableEntity : BaseEntity, IEditable
    {
        public Guid? Editor { get; set; }
        public DateTime? Edited { get; set; }
    }
}
