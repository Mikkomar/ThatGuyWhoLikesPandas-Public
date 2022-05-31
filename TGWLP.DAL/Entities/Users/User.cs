using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace TGWLP.DAL.Entities
{
    public class User : IdentityUser<Guid>
    {
        public User()
        {
        }

        public User(string userName) : base(userName)
        {
            this.UserName = userName;
        }

        [Required(ErrorMessage = "Validation_Required")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Validation_Required")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Validation_Required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public Guid Creator { get; set; }
        public DateTime Created { get; set; }
        public Guid Editor { get; set; }
        public DateTime? Edited { get; set; }

        [Display(Name = "FullName")]
        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
        public virtual List<UserRole> UserRoles { get; set; }
        
        [NotMapped]
        public List<UserRole> ActiveRoles
        {
            get
            {
                if (this.UserRoles != null)
                {
                    DateTime today = DateTime.Now.Date;
                    return this.UserRoles.Where(x => (!x.BeginDate.HasValue || x.BeginDate.Value <= today) && (!x.EndDate.HasValue || x.EndDate.Value >= today)).ToList();
                }
                else
                {
                    return new List<UserRole>();
                }
            }
        }

        [NotMapped]
        public List<RoleClaim> ActiveClaims
        {
            get
            {
                List<RoleClaim> claims = new List<RoleClaim>();
                foreach (var role in this.ActiveRoles)
                {
                    var roleClaims = role.Role.Claims.ToList();
                    claims.AddRange(roleClaims);
                }
                return claims;
            }
        }

        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public string Email_U { get; set; }
        [NotMapped]
        public string Email_D { get; set; }
        [NotMapped]
        public bool PrintMode { get; set; }
        //public override string Email
        //{
        //    get
        //    {
        //        return FirstName.Replace('Ä', 'A').Replace('Ö', 'O').ElementAt(0) + LastName.ToLower().Replace('ä', 'a').Replace('ö', 'o') + "@huiguan.app";
        //    }
        //}
    }
}
