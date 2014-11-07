using System;

namespace VirtoCommerce.Framework.Core.Caching
{
	[Serializable]
	public class CachedObject
	{
		public object Data { get; set; }

		public CachedObject(object data)
		{
			Data = data;
		}
	}

}
