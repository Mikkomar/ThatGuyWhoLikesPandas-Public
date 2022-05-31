
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TGWLP.Classes
{
    public static class Extensions
    {
        public static CultureLocalizer Localizer { get; set; }
        public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> list, string TextVariable, string ValueVariable, bool CreateEmptyItem = true, bool localizedName = false)
        {
            List<SelectListItem> Result = new List<SelectListItem>();
            if (CreateEmptyItem)
            {
                string emptyValue = string.Empty;
                //Type propertyType = list.First().GetType().GetProperty(ValueVariable).PropertyType;
                //if (propertyType == typeof(Guid))
                //{
                //    emptyValue = Guid.Empty.ToString();
                //}
                Result.Add(new SelectListItem()
                {
                    Text = Localizer["Select"],
                    Value = emptyValue
                });
            }
            foreach (T variable in list)
            {
                string textValue;
                if (localizedName)
                {
                    textValue = Localizer[variable.GetType().GetProperty(TextVariable).GetValue(variable).ToString()];
                }
                else
                {
                    textValue = variable.GetType().GetProperty(TextVariable).GetValue(variable).ToString();
                }
                Result.Add(new SelectListItem()
                {
                    Text = textValue,
                    Value = variable.GetType().GetProperty(ValueVariable).GetValue(variable).ToString(),
                });
            }
            return Result;
        }

        public static HtmlString RequiredFor<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            var attributes = new Dictionary<string, object>();
            var memberAccessExpression = (MemberExpression)expression.Body;
            var requiredAttribute = memberAccessExpression.Member.GetCustomAttributes(
            typeof(RequiredAttribute), true);
            if (requiredAttribute.Length > 0)
            {
                return new HtmlString("<span class=\"text-danger\">*</span>");
            }
            else
            {
                return new HtmlString("");
            }
        }
    }
}
