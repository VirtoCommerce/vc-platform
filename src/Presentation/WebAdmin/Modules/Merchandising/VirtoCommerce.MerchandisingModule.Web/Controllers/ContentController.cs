using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Frameworks.Tagging;
using VirtoCommerce.Foundation.Marketing.Services;
using VirtoCommerce.Framework.Web.Common;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using moduleModel = VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
	[RoutePrefix("api/mp/{language}/contents")]
    public class ContentController : ApiController
    {
        private readonly IDynamicContentService _dynamicContentService;
		public ContentController(IDynamicContentService dynamicContentService)
        {
			_dynamicContentService = dynamicContentService;
        }

        [HttpGet]
		[ResponseType(typeof(webModel.GenericSearchResult<webModel.DynamicContentItem>))]
        [Route("{placeholder}")]
		public IHttpActionResult GetDynamicContent(string placeHolder, [FromUri] string[] tags, string language = "en-us")
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
			var items = _dynamicContentService.GetItems(placeHolder, DateTime.Now, tagSet);

			var retVal = new webModel.GenericSearchResult<webModel.DynamicContentItem>()
			{
				Items = items.Select(x => x.ToWebModel()).ToList()
			};
			return Ok(retVal);
        }
    }
}
