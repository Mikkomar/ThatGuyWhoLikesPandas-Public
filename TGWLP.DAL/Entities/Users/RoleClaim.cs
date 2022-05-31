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
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual Role Role { get; set; }

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
