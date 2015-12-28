﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CoreModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CoreModule.Data.Repositories
{
	public interface IСommerceRepository : IRepository
	{
		IQueryable<FulfillmentCenter> FulfillmentCenters { get; }
		IQueryable<SeoUrlKeyword> SeoUrlKeywords { get; }
        IQueryable<Sequence> Sequences { get; }

        SeoUrlKeyword[] GetObjectSeoUrlKeywords(string objectType, string objectId);
    }
}
