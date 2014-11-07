using System;

namespace VirtoCommerce.Framework.Core.Infrastructure
{
	public class ModificationTrackingEntity : Entity, IModificationTrackingEntity
	{
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		public ModificationTrackingEntity()
		{
			Created = Modified = DateTime.UtcNow;
		}
	}
}
