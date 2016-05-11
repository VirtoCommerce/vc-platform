using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using VirtoCommerce.Platform.Data.Infrastructure;

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

		private readonly List<IInterceptor> _interceptors = new List<IInterceptor>();

		public InterceptionContext(IInterceptor[] interceptors)
		{
            //if (interceptors != null)
            {
                _interceptors = new List<IInterceptor>(interceptors);
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
