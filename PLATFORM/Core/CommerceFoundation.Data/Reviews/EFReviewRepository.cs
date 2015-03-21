using System.Linq;
using VirtoCommerce.Foundation.Reviews.Repositories;
using System.Data.Entity;
using VirtoCommerce.Foundation.Reviews;
using System.Data.Entity.ModelConfiguration.Conventions;
using VirtoCommerce.Foundation.Reviews.Model;
using VirtoCommerce.Foundation.Reviews.Factories;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.Foundation.Data.Reviews
{
    public class EFReviewRepository : EFRepositoryBase, IReviewRepository
    {
        public EFReviewRepository()
        {
        }

        public EFReviewRepository(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<EFReviewRepository>(null);
        }

        public EFReviewRepository(string nameOrConnectionString, IReviewEntityFactory entityFactory, IInterceptor[] interceptors = null)
            : base(nameOrConnectionString, entityFactory, interceptors: interceptors)
        {
            Database.SetInitializer(new ValidateDatabaseInitializer<EFReviewRepository>());

            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
        }

        [InjectionConstructor]
        public EFReviewRepository(IReviewEntityFactory entityFactory, IInterceptor[] interceptors = null)
            : this(ReviewConfiguration.Instance.Connection.SqlConnectionStringName, entityFactory, interceptors)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<Review>().Property(x => x.ReviewId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            MapEntity<Review>(modelBuilder, toTable: "Review");
            MapEntity<ReviewComment>(modelBuilder, toTable: "ReviewComment");
            MapEntity<ReviewFieldValue>(modelBuilder, toTable: "ReviewFieldValue");
            MapEntity<ReportAbuseElement>(modelBuilder, toTable: "ReportAbuseElement");
            MapEntity<ReportHelpfulElement>(modelBuilder, toTable: "ReportHelpfulElement");
            MapEntity<MediaContent>(modelBuilder, toTable: "MediaContent");

            // setting required fields. E.g. ReviewComment.ReviewId is required:
            modelBuilder.Entity<ReviewComment>().HasRequired(x => x.Review).WithMany(x => x.ReviewComments);
            modelBuilder.Entity<ReviewFieldValue>().HasRequired(x => x.Review).WithMany(x => x.ReviewFieldValues);
            modelBuilder.Entity<MediaContent>().HasRequired(x => x.Review).WithMany(x => x.MediaContents);

            base.OnModelCreating(modelBuilder);
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
