using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TGWLP.DAL.Entities;

namespace TGWLP.DAL.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public UserRole() : base()
        {

        }
        public override Guid RoleId { get; set; }
        public string Creator { get; set; }
        public DateTime Created { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Editor { get; set; }
        public DateTime? Edited { get; set; }
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
        public bool Active
        {
            get
            {
                return !this.EndDate.HasValue || !this.BeginDate.HasValue || (this.BeginDate.Value <= DateTime.Now.Date && this.EndDate.Value >= DateTime.Now.Date);
            }
        }

    }
}
