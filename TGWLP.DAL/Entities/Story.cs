using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGWLP.DAL.Entities
{
    public class Story : BaseEditableEntity
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string BookId { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime SaveDate { get; set; }
    }
}
