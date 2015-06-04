using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Platform.Core.Caching
{
	public interface ICacheKeyGenerator
	{
		CacheKey GetCacheKey(object keyData);
	}
}
