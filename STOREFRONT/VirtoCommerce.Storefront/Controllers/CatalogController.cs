using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _categoryService;
        private readonly WorkContext _workContext;

        public CatalogController(ICatalogService categoryService, WorkContext workContext)
        {
            _categoryService = categoryService;
            _workContext = workContext;
        }

        public ActionResult Category(string categoryid)
        {
            //_workContext.CurrentCategory = await _categoryService.GetCategoryById(categoryid, _workContext.CurrentStore.Catalog, _workContext.CurrentLanguage.CultureName, _workContext.CurrentCurrency.Code);
            return View("collection", _workContext);
        }

        public ActionResult AllCategories()
        {
            //_workContext.AllCategories = await _categoryService.GetCategoriesByCatalog(_workContext.CurrentStore.Catalog);
            return View("collection.list", _workContext);
        }
    }
}