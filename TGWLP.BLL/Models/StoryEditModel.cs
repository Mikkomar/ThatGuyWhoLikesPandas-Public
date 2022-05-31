using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TGWLP.BLL.Models
{
    public class StoryEditModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Text")]
        public string Text { get; set; }
        [Display(Name = "BookId")]
        public string BookId { get; set; }
        public DateTime? PublishDate { get; set; }
    }
}
