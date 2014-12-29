using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Frameworks.Common;
using VirtoCommerce.ManagementClient.Marketing.Model;
using System.Collections.ObjectModel;

namespace VirtoCommerce.ManagementClient.Marketing.Services
{
    public class MockMarketingRepository : IMarketingRepository
    {
        private List<Promotion> MockPromotionList;


        public MockMarketingRepository()
        {
            PopulateTestData();
        }

        private void PopulateTestData()
        {
            MockPromotionList = new List<Promotion>();

            MockPromotionList.Add(new CatalogPromotion { Name = "promotion 1", Priority = 2, CatalogId = "Sony" });
            MockPromotionList.Add(new CatalogPromotion { Name = "promotion 2", Priority = 2, CatalogId = "Sony" });
            MockPromotionList.Add(new CatalogPromotion { Name = "promotion 3", Priority = 2, CatalogId = "Sony" });
            MockPromotionList.Add(new CatalogPromotion { Name = "promotion 4", Priority = 2, CatalogId = "Sony" });
        }

        #region IMarketingRepository Members

        public IQueryable<VirtoCommerce.Foundation.Marketing.Model.Promotion> Promotions
        {
            get { return MockPromotionList.AsQueryable(); }
        }

        public IQueryable<PromotionReward> PromotionRewards
        {
            get { throw new NotImplementedException(); }
        }

        public IQueryable<VirtoCommerce.Foundation.Marketing.Model.CouponSet> CouponSets
        {
            get { throw new NotImplementedException(); }
        }

        public IQueryable<VirtoCommerce.Foundation.Marketing.Model.Coupon> Coupons
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IRepository Members

        public VirtoCommerce.Foundation.Frameworks.IUnitOfWork UnitOfWork
        {
            get { throw new NotImplementedException(); }
        }

        public void Attach<T>(T item) where T : class
        {
            // throw new NotImplementedException();
        }

        public bool IsAttachedTo<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public void Add<T>(T item) where T : class
        {
        }

        public void Update<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public void Remove<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAsQueryable<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public void Refresh(IEnumerable collection)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion



        public IQueryable<PromotionUsage> PromotionUsages
        {
            get { throw new NotImplementedException(); }
        }
    }
}
