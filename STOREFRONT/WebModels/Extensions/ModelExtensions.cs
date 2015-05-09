#region
using System.Linq;
using System.Web.ModelBinding;

#endregion

namespace VirtoCommerce.Web.Models.Extensions
{
    public static class ModelExtensions
    {
        #region Public Methods and Operators
        public static string[] Errors(this ModelStateDictionary model)
        {
            var errors = model.Select(m => m.Value.Errors.Select(e => e.ErrorMessage)).Select(x => x.ToString());
            return errors.ToArray();
        }
        #endregion
    }
}