using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Infrastructure.Interceptors
{
	public class EntityPrimaryKeyGeneratorInterceptor : ChangeInterceptor<Entity>
	{
		public override void OnBeforeInsert(DbEntityEntry entry, Entity entity)
		{
			base.OnBeforeInsert(entry, entity);

			if(entity.IsTransient())
			{
				entity.Id = Guid.NewGuid().ToString();
			}
		}

	}
}
