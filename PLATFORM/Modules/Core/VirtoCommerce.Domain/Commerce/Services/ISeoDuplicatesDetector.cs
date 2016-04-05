using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Commerce.Services
{
    public interface ISeoDuplicatesDetector
    {
        IEnumerable<SeoInfo> DetectSeoDuplicates(string objectType, string objectId, IEnumerable<SeoInfo> allSeoDuplicates);
    }
}
