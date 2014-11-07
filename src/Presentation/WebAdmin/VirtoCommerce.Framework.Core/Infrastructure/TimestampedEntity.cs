using System;

namespace VirtoCommerce.Framework.Core.Infrastructure
{
	public abstract class TimestampedEntity : Entity
	{
		public TimestampedEntity()
		{
			Created = DateTime.UtcNow;
		}

		public DateTime Created { get; set; }
	}
}
