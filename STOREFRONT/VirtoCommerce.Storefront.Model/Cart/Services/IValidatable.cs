using System.Collections.Generic;
using VirtoCommerce.Storefront.Model.Cart.ValidationErrors;

namespace VirtoCommerce.Storefront.Model.Cart.Services
{
    public interface IValidatable
    {
        ICollection<ValidationError> ValidationErrors { get; }

        ICollection<ValidationError> ValidationWarnings { get; }
    }
}