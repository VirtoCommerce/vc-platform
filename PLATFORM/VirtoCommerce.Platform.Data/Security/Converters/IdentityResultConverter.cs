using System.Linq;
using Microsoft.AspNet.Identity;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Data.Security.Converters
{
    public static class IdentityResultConverter
    {
        public static SecurityResult ToCoreModel(this IdentityResult dataModel)
        {
            var result = new SecurityResult();
            result.InjectFrom(dataModel);

            if (dataModel.Errors != null)
                result.Errors = dataModel.Errors.ToArray();

            return result;
        }
    }
}
