using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.Caching
{
	public interface ICacheKeyGenerator
	{
		CacheKey GetCacheKey(object keyData);
	}
}
