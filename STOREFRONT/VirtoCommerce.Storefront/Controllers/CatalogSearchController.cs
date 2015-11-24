using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    public class CatalogSearchController : StorefrontControllerBase
    {
        private readonly ICatalogService _categoryService;

        public CatalogSearchController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, ICatalogService categoryService)
            : base(workContext, urlBuilder)
        {
            _categoryService = categoryService;

        }


    }
}