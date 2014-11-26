using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    using System.Web.Http;

    using VirtoCommerce.Foundation.Frameworks.Tagging;
    using VirtoCommerce.Foundation.Marketing.Services;
    using VirtoCommerce.Framework.Web.Common;
    using VirtoCommerce.MerchandisingModule.Data.Convertors;
    using VirtoCommerce.MerchandisingModule.Model;

    [RoutePrefix("api/contents")]
    public class ContentController : ApiController
    {
        private readonly IDynamicContentService _service = null;
        public ContentController(IDynamicContentService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{placeholder}")]
        public ResponseCollection<DynamicContentItem> GetDynamicContent(string placeHolder)
        {
            // TODO: add caching
            var items = _service.GetItems(placeHolder, DateTime.Now, new TagSet());

            var response = new ResponseCollection<DynamicContentItem>();
            if (items != null)
            {
                response.Items.AddRange(items.Select(x => x.ToModuleModel()).ToArray());
            }

            return response;
        }
    }
}
