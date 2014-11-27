using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    using System.Web.Http;

    using VirtoCommerce.Foundation.Frameworks.Extensions;
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
        public ResponseCollection<DynamicContentItem> GetDynamicContent(string placeHolder, [FromUri] string[] tags)
        {
            var tagSet = new TagSet();

            if (tags != null)
            {
                foreach (var tagArray in tags.Select(tag => tag.Split(new[] { ':' })))
                {
                    tagSet.Add(tagArray[0], tagArray[1]);
                }
            }

            // TODO: add tags ?tags={users:[id1,id2]}
            // TODO: add caching
            var items = _service.GetItems(placeHolder, DateTime.Now, tagSet);

            var response = new ResponseCollection<DynamicContentItem>();
            if (items != null)
            {
                response.Items.AddRange(items.Select(x => x.ToModuleModel()).ToArray());
            }

            return response;
        }
    }
}
