﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Web.Caching
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
