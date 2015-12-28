﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Commerce.Model
{
	public interface ISeoSupport 
	{
        string Id { get; set; }
		ICollection<SeoInfo> SeoInfos { get; set; }
	}
}
