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

        public CommerceRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, null, interceptors)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<FulfillmentCenter>().HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<FulfillmentCenter>().ToTable("FulfillmentCenter");

            modelBuilder.Entity<SeoUrlKeyword>().HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<SeoUrlKeyword>().ToTable("SeoUrlKeyword");

            modelBuilder.Entity<Sequence>().HasKey(x => x.ObjectType).Property(x => x.ObjectType);
            modelBuilder.Entity<Sequence>().ToTable("Sequence");


            modelBuilder.Entity<Currency>().HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<Currency>().ToTable("Currency");


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
        public IQueryable<Currency> Currencies
        {
            get { return GetAsQueryable<Currency>(); }
        }

        public SeoUrlKeyword[] GetSeoByIds(string[] ids)
        {
            return SeoUrlKeywords.Where(x => ids.Contains(x.Id)).OrderBy(x => x.Keyword).ToArray();
        }
        public SeoUrlKeyword[] GetObjectSeoUrlKeywords(string objectType, string objectId)
        {
            return SeoUrlKeywords.Where(x => x.ObjectId == objectId && x.ObjectType == objectType).OrderBy(x => x.Language).ToArray();
        }

        #endregion


    }

}
