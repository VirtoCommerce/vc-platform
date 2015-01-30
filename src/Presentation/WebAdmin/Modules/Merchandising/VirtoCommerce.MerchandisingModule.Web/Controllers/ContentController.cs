using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Frameworks.Tagging;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Services;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp/{language}/contents")]
    public class ContentController : ApiController
    {
        private readonly Func<IDynamicContentService> _dynamicContentService;
        public ContentController(Func<IDynamicContentService> dynamicContentService)
        {
            _dynamicContentService = dynamicContentService;
        }

        [HttpGet]
        [ResponseType(typeof(webModel.ResponseCollection<webModel.DynamicContentItemGroup>))]
        [Route("{placeHolder}")]
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

            //Mutiple placeholders can be requested
            var placeHolders = placeHolder.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            var groups = new List<webModel.DynamicContentItemGroup>();

            foreach (var holder in placeHolders)
            {
                var group = new webModel.DynamicContentItemGroup(holder);

                var results = _dynamicContentService().GetItems(holder, DateTime.Now, tagSet);

                if (results != null && results.Any())
                {
                    group.Items.AddRange(results.Select(x => x.ToWebModel()));
                    groups.Add(group);
                }
            }

            if (groups.Any())
            {
                var retVal = new webModel.ResponseCollection<webModel.DynamicContentItemGroup>
                {
                    Items = groups,
                    TotalCount = groups.Count()
                };

                return Ok(retVal);
            }

            return StatusCode(HttpStatusCode.NoContent);

        }
    }
}

