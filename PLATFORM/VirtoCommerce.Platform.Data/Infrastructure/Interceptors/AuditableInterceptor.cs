using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Infrastructure.Interceptors
{
	public class AuditableInterceptor : ChangeInterceptor<IAuditable>
	{
		public override void OnBeforeInsert(DbEntityEntry entry, IAuditable item)
		{
			base.OnBeforeInsert(entry, item);

			var currentTime = DateTime.UtcNow;

			item.CreatedDate =  currentTime;
			item.ModifiedDate = currentTime;
			item.CreatedBy = item.ModifiedBy = CurrentPrincipal.GetCurrentUserName();
		}

		public override void OnBeforeUpdate(DbEntityEntry entry, IAuditable item)
		{
			base.OnBeforeUpdate(entry, item);
			var currentTime = DateTime.UtcNow;
			item.ModifiedDate = currentTime;
			item.CreatedBy = CurrentPrincipal.GetCurrentUserName();
		}

		public override void OnAfterInsert(DbEntityEntry entry, IAuditable item)
		{
			base.OnAfterInsert(entry, item);
		}

	    
	}
}
