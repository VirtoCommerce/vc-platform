using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Logging;
using VirtoCommerce.Foundation.Frameworks.Tagging;
using VirtoCommerce.Foundation.Search;

namespace VirtoCommerce.Foundation.Catalogs.Services
{
	[ServiceContract(Name = "CatalogService", Namespace = "http://services.virtocommerce.com/catalogservice")]
	public interface ICatalogService
	{
		[OperationContract]
		[WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
		CatalogItemSearchResults SearchItems(string scope, CatalogItemSearchCriteria criteria);
    }
}
