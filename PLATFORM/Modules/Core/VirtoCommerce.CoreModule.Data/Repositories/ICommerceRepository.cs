using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CoreModule.Data.Model;

namespace VirtoCommerce.CoreModule.Data.Repositories
{
    public interface IСommerceRepository : VirtoCommerce.Platform.Core.Common.IRepository
    {
        IQueryable<FulfillmentCenter> FulfillmentCenters { get; }
        IQueryable<SeoUrlKeyword> SeoUrlKeywords { get; }
        IQueryable<Sequence> Sequences { get; }
        IQueryable<Currency> Currencies { get; }

        SeoUrlKeyword[] GetSeoByIds(string[] ids);
        SeoUrlKeyword[] GetObjectSeoUrlKeywords(string objectType, string objectId);
    }
}
