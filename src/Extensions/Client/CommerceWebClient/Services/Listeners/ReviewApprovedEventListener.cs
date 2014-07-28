using System;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks.Events;
using VirtoCommerce.Foundation.Reviews.Model;

namespace VirtoCommerce.Web.Client.Services.Listeners
{
    public class ReviewApprovedEventListener : ChangeEntityEventListener<Review>
    {
        private readonly ICatalogRepository _catalogRepository;

        public ReviewApprovedEventListener(ICatalogRepository catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        //Update item modified date so that reviews would reindex for it
        public override void OnBeforeUpdate(Review review, EntityEventArgs e)
        {
            if (e.CurrentValues.GetValue<int>("Status") == (int) ReviewStatus.Approved
                && e.OriginalValues.GetValue<int>("Status") != (int) ReviewStatus.Approved)
            {
                var item = _catalogRepository.Items.FirstOrDefault(i => i.ItemId == review.ItemId);
                if (!ReferenceEquals(item, null))
                {
                    item.LastModified = DateTime.UtcNow;
                    _catalogRepository.UnitOfWork.Commit();
                }
            }
        }
    }
}
