using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Orders.Model.Taxes;

namespace VirtoCommerce.Foundation.Orders.Repositories
{
    public interface ITaxRepository : IRepository
    {
        IQueryable<Tax> Taxes { get; }
    
    }
}
