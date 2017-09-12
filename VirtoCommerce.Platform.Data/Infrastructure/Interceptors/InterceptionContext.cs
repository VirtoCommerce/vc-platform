using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Infrastructure.Interceptors
{
    public class InterceptionContext
    {
        public EFRepositoryBase DatabaseContext { get; internal set; }
        public ObjectContext ObjectContext { get; internal set; }
        public ObjectStateManager ObjectStateManager { get; internal set; }
        public DbChangeTracker ChangeTracker { get; internal set; }
        public IEnumerable<DbEntityEntry> Entries { get; internal set; }
        public ILookup<EntityState, DbEntityEntry> EntriesByState { get; internal set; }

        /// <summary>
        /// Entities deleted by batch command
        /// </summary>
        public IList<Entity> BatchDeletedEntities { get; internal set; }

        private readonly List<IInterceptor> _interceptors = new List<IInterceptor>();

        public InterceptionContext(IInterceptor[] interceptors)
        {
            if (interceptors != null)
            {
                _interceptors.AddRange(interceptors);
            }
        }

        public void Before()
        {
            var interceptors = _interceptors;
            interceptors.ForEach(intercept => intercept.Before(this));
        }

        public void After()
        {
            var interceptors = _interceptors;
            interceptors.ForEach(intercept => intercept.After(this));
        }
    }
}
