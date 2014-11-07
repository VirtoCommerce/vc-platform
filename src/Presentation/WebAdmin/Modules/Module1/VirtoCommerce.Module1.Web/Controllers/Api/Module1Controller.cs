using System.Web.Http;

namespace VirtoCommerce.Module1.Web.Controllers.Api
{
	public class Module1Controller : ApiController
	{
		// GET: api/module1/
		[HttpGet]
		public IHttpActionResult Get()
		{
			return Ok(new[] { "Hello world!" });
		}
	}
}
