using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.ServiceModel;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Factories;

namespace VirtoCommerce.Foundation.Stores.Services
{
	[ServiceContract]
    public interface IStoreService
    {
        DateTime CurrentDateTime { get; set; }
        IQueryable<Store> Stores { get; }
    }
}
