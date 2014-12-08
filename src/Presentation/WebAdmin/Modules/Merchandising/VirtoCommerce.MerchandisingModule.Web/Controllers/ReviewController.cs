using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using Microsoft.Practices.Unity;
using VirtoCommerce.CatalogModule.Services;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.Foundation.Reviews.Repositories;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.MerchandisingModule.Web.Binders;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.MerchandisingModule.Web.Model;
using foundation = VirtoCommerce.Foundation.Reviews.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
	[RoutePrefix("api/mp/{catalog}/{language}/reviews")]
	public class ReviewController : ApiController
	{
		private readonly Func<IReviewRepository> _reviewRepositoryFactory;
		public ReviewController([Dependency("MP")]Func<IReviewRepository> reviewRepositoryFactory)
		{
			_reviewRepositoryFactory = reviewRepositoryFactory;
		}


		[HttpGet]
		[Route("~/api/mp/{catalog}/{language}/products/{productId}/reviews")]
		[ResponseType(typeof(Review[]))]
		public IHttpActionResult GetAllProductReviews(string catalog, string language, string productId)
		{
			Review[] retVal = null;
			using (var repository = _reviewRepositoryFactory())
			{
				var reviews = repository.Reviews.Where(x => (string.IsNullOrEmpty(productId) || x.ItemId == productId) && x.Status == (int)foundation.ReviewStatus.Approved)
										        .ExpandAll()
											    .ToArray();
				retVal = reviews.Select(x => x.ToWebModel()).ToArray();
			}
			return Ok(retVal);

		}

	}
}
