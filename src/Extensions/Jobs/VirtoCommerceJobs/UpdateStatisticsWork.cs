using System.Data.Entity;
using VirtoCommerce.Foundation.Data.Orders;
using VirtoCommerce.Foundation.Orders.Repositories;

namespace VirtoCommerce.Scheduling.Jobs
{
    /// <summary>
    /// Update data in Statistic datatable.
    /// </summary>
    public class UpdateStatisticsWork : IJobActivity
    {
        readonly Database database;

        public UpdateStatisticsWork(IOrderRepository orderRepository)
        {
            var efRep = orderRepository as EFOrderRepository;
            if (efRep != null)
            {
                database = efRep.Database;
            }
        }
        public void Execute(IJobContext context)
        {
            if (database != null)
                database.ExecuteSqlCommand("EXEC UpdateStatistics");
        }
    }
}
