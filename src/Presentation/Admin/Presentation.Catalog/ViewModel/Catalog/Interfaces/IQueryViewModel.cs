using System;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Model;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
    public interface IQueryViewModel : IViewModel
    {
        ItemFilter InnerItem { get; }

        bool Validate();
        linq.Expression<Func<Item, bool>> GetCompleteExpression();
    }
}
