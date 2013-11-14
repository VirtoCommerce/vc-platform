using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Reviews.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Reviews.Model;
using VirtoCommerce.Foundation.Frameworks.Caching;
using VirtoCommerce.Foundation.Reviews.Factories;
using VirtoCommerce.Foundation.Security.Services;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Reviews;

namespace VirtoCommerce.Foundation.Data.Reviews
{
	public class DSReviewClient : DSClientBase, IReviewRepository
	{
		[InjectionConstructor]
		public DSReviewClient(IReviewEntityFactory entityFactory, ISecurityTokenInjector tokenInjector, IServiceConnectionFactory connFactory)
			: base(connFactory.GetConnectionString(ReviewConfiguration.Instance.Connection.DataServiceUri), entityFactory, tokenInjector)
		{
		}

        public DSReviewClient(Uri serviceUri, IReviewEntityFactory factory, ISecurityTokenInjector tokenInjector)
			:base(serviceUri, factory, tokenInjector)
		{
		}

		#region IReviewRepository Members

		public IQueryable<Review> Reviews
		{
			get { return GetAsQueryable<Review>(); }
		}

		public IQueryable<ReviewComment> ReviewComments
		{
			get { return GetAsQueryable<ReviewComment>(); }
		}

		public IQueryable<ReviewFieldValue> ReviewFieldValues
		{
			get { return GetAsQueryable<ReviewFieldValue>(); }
		}

		public IQueryable<MediaContent> MediaContents
		{
			get { return GetAsQueryable<MediaContent>(); }
		}


		public IQueryable<ReportAbuseElement> ReportAbuseElements
		{
			get { return GetAsQueryable<ReportAbuseElement>(); }
		}

		public IQueryable<ReportHelpfulElement> ReportHelpfulElements
		{
			get { return GetAsQueryable<ReportHelpfulElement>(); }
		}

		#endregion
	}
}
