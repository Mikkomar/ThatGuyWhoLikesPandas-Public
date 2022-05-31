using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TGWLP.DAL.Entities
{
    public abstract class BaseLocalizedEntity
    {
        protected abstract object GetLocalizedValueOfProperty<TProperty>(Expression<Func<TProperty>> property);
    }
}
