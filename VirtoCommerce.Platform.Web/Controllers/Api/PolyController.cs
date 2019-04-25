using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/platform/poly")]
    public class PolyController : ApiController
    {
        public PolyController()
        {
        }

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(BaseClass[]))]
        public IHttpActionResult GetPolyTypes()
        {
            //return Ok(new BaseClassGroup
            //{
            //    Name = "GroupObject2",
            //    AdditionalInfo = "Add info on this object",
            //    Items = new[]
            //    {
            //            new  BaseClass { Name = "ChildItem1"},
            //            new  BaseClass { Name = "ChildItem2"},
            //        }
            //});
            return Ok(new BaseClass[]
            {
                new BaseClass { Name = "BaseObject1"},
                new BaseClass { Name = "BaseObject2"},
            new BaseClassGroup
            {
                Name = "GroupObject2",
                AdditionalInfo = "Add info on this object",
                Items = new[]
                {
                        new  BaseClass { Name = "ChildItem1"},
                        new  BaseClass { Name = "ChildItem2"},
                    }
            },
            });
        }
    }

    public class BaseClass
    {
        public string Name { get; set; }
        public string Type
        {
            get
            {
                return GetType().Name;
            }
        }
    }

    public class BaseClassGroup : BaseClass
    {
        public ICollection<BaseClass> Items { get; set; }
        public string AdditionalInfo { get; set; }
    }

}
