using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TGWLP.DAL.Attributes;

namespace TGWLP.DAL.Entities
{
    public class AppClaim : BaseEditableEntity
    {
        [Display(Name = "ClaimType")]
        public string ClaimType { get; set; }
        [Display(Name = "ClaimValue")]
        public string ClaimValue { get; set; }
        [LogHistory]
        [Display(Name = "NameEn")]
        public string NameEn { get; set; }
        [LogHistory]
        [Display(Name = "NameFi")]
        public string NameFi { get; set; }
        [LogHistory]
        [Display(Name = "DescriptionEn")]
        public string DescriptionEn { get; set; }
        [LogHistory]
        [Display(Name = "DescriptionFi")]
        public string DescriptionFi { get; set; }

        [NotMapped]
        public bool Selected { get; set; }

        public string Name
        {
            get
            {
                return (string)this.GetLocalizedValueOfProperty(() => this.Name);
            }
        }

        public string Description
        {
            get
            {
                return (string)this.GetLocalizedValueOfProperty(() => this.Description);
            }
        }
    }
}
