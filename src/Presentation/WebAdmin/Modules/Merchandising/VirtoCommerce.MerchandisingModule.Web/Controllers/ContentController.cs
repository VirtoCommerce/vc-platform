using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Frameworks.Tagging;
using VirtoCommerce.Foundation.Marketing.Services;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp/{language}/contents")]
    public class ContentController : ApiController
    {
        private readonly IDynamicContentService _dynamicContentService;
		public ContentController([Dependency("MP")] IDynamicContentService dynamicContentService)
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

            if (items != null)
            {

                var retVal = new webModel.GenericSearchResult<webModel.DynamicContentItem>
                {
                    Items = items.Select(x => x.ToWebModel()).ToList(),
                    TotalCount = items.Count()
                };

                return Ok(retVal);
            }

            return StatusCode(HttpStatusCode.NoContent);

        }
    }
}

