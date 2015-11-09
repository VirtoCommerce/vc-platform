using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// array of strings if the form was not submitted successfully. The strings returned depend on which fields of the form were left empty or contained errors. Possible values are:
    /// </summary>
    public class FormErrors : Drop, IEnumerable<string>
    {
      
        public FormErrors(ModelStateDictionary modelState)
        {
            _fields = modelState.Keys;
            Messages = modelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(y => y.ErrorMessage).FirstOrDefault());
        }

        private ICollection<string> _fields { get; set; }

        public IDictionary<string, string> Messages { get; set; }

        public IEnumerator<string> GetEnumerator()
        {
            foreach (string field in _fields)
            {
                yield return field;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (string field in _fields)
            {
                yield return field;
            }
        }
    }
}
