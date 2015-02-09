using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.Foundation.Reviews.Repositories;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.MerchandisingModule.Web.Binders;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.MerchandisingModule.Web.Model;
using foundation = VirtoCommerce.Foundation.Reviews.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
	public class ReviewController : ApiController
	{
		private readonly Func<IReviewRepository> _reviewRepositoryFactory;
		public ReviewController(Func<IReviewRepository> reviewRepositoryFactory)
		{
			_reviewRepositoryFactory = reviewRepositoryFactory;
		}


		[HttpGet]
		[Route("~/api/mp/{language}/products/{productId}/reviews")]
		[ResponseType(typeof(ResponseCollection<Review>))]
		public IHttpActionResult GetAllProductReviews(string language, string productId)
		{
            var retVal = new ResponseCollection<Review>();
			using (var repository = _reviewRepositoryFactory())
			{
				var reviews = repository.Reviews.Where(x => (string.IsNullOrEmpty(productId) || x.ItemId == productId) && x.Status == (int)foundation.ReviewStatus.Approved)
										        .ExpandAll()
											    .ToArray();
				retVal.Items = reviews.Select(x => x.ToWebModel()).ToArray();
			    retVal.TotalCount = retVal.Items.Count;
			}
			return Ok(retVal);

		}

	}
}
