using System.Collections.Generic;
using VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.Domain.Catalog.Services
{
    public interface IOutlineService
    {
        void FillOutlinesForObjects(IEnumerable<IHasOutlines> objects, string catalogId);
    }
}
