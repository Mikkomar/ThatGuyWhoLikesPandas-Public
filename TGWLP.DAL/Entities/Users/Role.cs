using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace TGWLP.DAL.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public Role() : base()
        {

        }
        public Role (string roleName) : base(roleName)
        {

        }

        public string NameEn { get; set; }
        public string NameFi { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Creator { get; set; }
        public DateTime Created { get; set; }
        public string Editor { get; set; }
        public DateTime? Edited { get; set; }

        public virtual IEnumerable<UserRole> UserRoles { get; set; }
        public virtual IEnumerable<RoleClaim> Claims { get; set; }
        //public string LocalizedName
        //{
        //    get
        //    {
        //        return (string)this.GetLocalizedValueOfProperty(() => this.LocalizedName);
        //    }
        //}

        protected object GetLocalizedValueOfProperty<TProperty>(Expression<Func<TProperty>> property)
        {
            var currentCulture = CultureInfo.CurrentCulture.Name;
            var propertyInfo = ((MemberExpression)property.Body).Member as PropertyInfo;
            object localizedValue = null;
            switch (currentCulture)
            {
                case "en":
                    localizedValue = this.GetType().GetProperty(propertyInfo.Name + "En").GetValue(this);
                    break;
                case "fi":
                    localizedValue = this.GetType().GetProperty(propertyInfo.Name + "Fi").GetValue(this);
                    break;
                default:
                    localizedValue = this.GetType().GetProperty(propertyInfo.Name + "En").GetValue(this);
                    break;
            }
            if (localizedValue == null || string.IsNullOrEmpty(localizedValue.ToString()))
            {
                localizedValue = this.GetType().GetProperty(propertyInfo.Name + "En").GetValue(this);
            }

            return localizedValue;
        }
    }
}
