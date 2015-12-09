using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.CoreModule.Data.Model;

namespace VirtoCommerce.CoreModule.Data.Repositories
{
	public class CommerceRepositoryImpl : EFRepositoryBase, IСommerceRepository
	{
		public CommerceRepositoryImpl()
		{
		}

		public CommerceRepositoryImpl(string nameOrConnectionString)
			: this(nameOrConnectionString, null)
		{
		}
		public CommerceRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{
			Configuration.LazyLoadingEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			MapEntity<FulfillmentCenter>(modelBuilder, toTable: "FulfillmentCenter");
			MapEntity<SeoUrlKeyword>(modelBuilder, toTable: "SeoUrlKeyword");
            MapEntity<Sequence>(modelBuilder, toTable: "Sequence");

			base.OnModelCreating(modelBuilder);
		}

		#region IСommerceRepository Members

		public IQueryable<FulfillmentCenter> FulfillmentCenters
		{
			get { return GetAsQueryable<FulfillmentCenter>(); }
		}
		public IQueryable<SeoUrlKeyword> SeoUrlKeywords
		{
			get { return GetAsQueryable<SeoUrlKeyword>(); }
		}
        public IQueryable<Sequence> Sequences
        {
            get { return GetAsQueryable<Sequence>(); }
        }

        public SeoUrlKeyword[] GetObjectSeoUrlKeywords(string objectType, string objectId)
        {
            return SeoUrlKeywords.Where(x => x.ObjectId == objectId && x.ObjectType == objectType).OrderBy(x => x.Language).ToArray();
        }

        #endregion


    }

}
