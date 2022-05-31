using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using TGWLP.DAL.Interfaces;

namespace TGWLP.DAL.Entities
{
    public class BaseEntity : BaseLocalizedEntity, IEntity
    {
        public Guid Id { get; set; }
        [NotMapped]
        public Guid Creator { get; set; }
        [NotMapped]
        public DateTime Created { get; set; }

        public bool IsNewEntity()
        {
            return this.Id == Guid.Empty;
        }

        protected override object GetLocalizedValueOfProperty<TProperty>(Expression<Func<TProperty>> property)
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
