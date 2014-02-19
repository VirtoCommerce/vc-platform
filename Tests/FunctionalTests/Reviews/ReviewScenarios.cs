using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Data.Reviews;
using VirtoCommerce.Foundation.Reviews.Model;
using VirtoCommerce.Foundation.Reviews.Repositories;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.PowerShell.DatabaseSetup;
using Xunit;
using VirtoCommerce.Foundation.Reviews.Factories;
using FunctionalTests.TestHelpers;
using VirtoCommerce.Foundation.Data.Reviews.Migrations;
using VirtoCommerce.Foundation.Security.Services;
using System.ServiceModel;
using System.Threading;
using System.Data.Services;
using System.Data.Services.Common;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data;
using System.IO;

namespace FunctionalTests.Reviews
{
    [JsonSupportBehavior]
    public class TestDSReviewService : ReviewDataService
    {
        protected override EFReviewRepository CreateRepository()
        {
            return new EFReviewRepository("ReviewsTest", new ReviewEntityFactory());
        }
    }

    [Variant(RepositoryProvider.EntityFramework)]
    [Variant(RepositoryProvider.DataService)]
	public class ReviewScenarios : FunctionalTestBase, IDisposable
    {
        private TestDataService _Service = null;

        #region Infrastructure/setup

        private readonly string _databaseName;
        private readonly object _previousDataDirectory;

        public ReviewScenarios()
        {
            _previousDataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory");
            AppDomain.CurrentDomain.SetData("DataDirectory", TempPath);
            _databaseName = "ReviewsTest";
            _Service = new TestDataService(typeof(TestDSReviewService));
        }

        public void Dispose()
        {
            try
            {
                // Ensure LocalDb databases are deleted after use so that LocalDb doesn't throw if
                // the temp location in which they are stored is later cleaned.
                using (var context = new EFReviewRepository(_databaseName))
                {
                    context.Database.Delete();
                }

                _Service.Dispose();
            }
            finally
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", _previousDataDirectory);
            }
        }

        #endregion

        [RepositoryTheory]
        public void Can_save_review_comments()
        {         
            var client = GetRepository();

            var reviewId = Guid.NewGuid().ToString();
            var customerId = Guid.NewGuid().ToString();
            Review dbReviewreview = new Review()
            {
                ReviewId = reviewId,
                AuthorId = customerId,
                AuthorLocation = "Los Angeles",
                AuthorName = "NickName",
                ItemId = "someitem",
                ItemUrl = "url",
                OverallRating = 5,
                Title = "comment",
                IsVerifiedBuyer = true,
                Status = (int)ReviewStatus.Pending
            };

            client.Add(dbReviewreview);
            client.UnitOfWork.Commit();

            //RefreshClient
            RefreshRepository(ref client);

            var loadedReview = client.Reviews.Where(r => r.ReviewId == reviewId).SingleOrDefault();

            //client.Attach(loadedReview);

            ReviewComment reviewComment = new ReviewComment();

            reviewComment.AuthorId = customerId;
            reviewComment.AuthorLocation = "Los Angeles";
            reviewComment.AuthorName = "Author";
            reviewComment.Comment = "Comment";
            reviewComment.ReviewCommentId = Guid.NewGuid().ToString();
            reviewComment.Status = (int)ReviewStatus.Pending;
            loadedReview.ReviewComments.Add(reviewComment);

            //client.Update(loadedReview);
            client.UnitOfWork.Commit();

            RefreshRepository(ref client);

            loadedReview = (from r in client.Reviews where r.ReviewId == reviewId select r).ExpandAll().SingleOrDefault();

            // test delete from collection, object should be removed from db as well
            loadedReview.ReviewComments.Remove(loadedReview.ReviewComments[0]);
            client.UnitOfWork.Commit();

            RefreshRepository(ref client);

            loadedReview = (from r in client.Reviews where r.ReviewId == reviewId select r).ExpandAll().SingleOrDefault();

            Assert.True(loadedReview.ReviewComments.Count == 0);

            // test saving just a comment by itself
            var reviewComment2 = new ReviewComment();

            reviewComment2.AuthorId = customerId;
            reviewComment2.AuthorLocation = "Los Angeles";
            reviewComment2.AuthorName = "Author";
            reviewComment2.ReviewId = reviewId;
            reviewComment2.Comment = "Comment";
            reviewComment2.ReviewCommentId = Guid.NewGuid().ToString();
            reviewComment2.Status = (int)ReviewStatus.Pending;
            client.Add(reviewComment2);
            client.UnitOfWork.Commit();

            var comment2 = (from r in client.ReviewComments where r.ReviewId == reviewId && r.ReviewCommentId == reviewComment2.ReviewCommentId select r).SingleOrDefault();
            Assert.True(comment2 != null);

            // now modify it
            reviewComment2.AuthorName = "Author2";
            client.UnitOfWork.Commit();

            comment2 = (from r in client.ReviewComments where r.ReviewId == reviewId && r.ReviewCommentId == reviewComment2.ReviewCommentId select r).SingleOrDefault();
            Assert.True(comment2 != null);
        }

        private IReviewRepository GetRepository()
        {
            EnsureDatabaseInitialized(() => new EFReviewRepository(_databaseName),
				() => Database.SetInitializer(new SetupMigrateDatabaseToLatestVersion<EFReviewRepository, Configuration>()));

            if (RepositoryProvider == RepositoryProvider.DataService)
            {
                return new DSReviewClient(_Service.ServiceUri, new ReviewEntityFactory(), null);
            }
            else
            {
                return new EFReviewRepository(_databaseName);
            }
        }

        private void RefreshRepository(ref IReviewRepository client)
        {
            client.Dispose();
            client = null;
            GC.Collect();
            client = GetRepository();
        }
    }
}