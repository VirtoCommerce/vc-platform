using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks
{
    public interface IModifiedDateTimeFields
    {
        DateTime? LastModified { get; set; }
        DateTime? Created { get; set; }
    }
}
