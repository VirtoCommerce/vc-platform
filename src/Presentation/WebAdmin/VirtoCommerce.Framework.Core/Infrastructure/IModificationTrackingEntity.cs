using System;

namespace VirtoCommerce.Framework.Core.Infrastructure
{
	public interface IModificationTrackingEntity
	{
		DateTime Created { get; set; }
		DateTime Modified { get; set; }
	}
}
