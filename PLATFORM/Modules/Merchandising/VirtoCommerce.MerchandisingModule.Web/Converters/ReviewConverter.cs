using System.Linq;
using Omu.ValueInjecter;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;
using foundation = VirtoCommerce.Foundation.Reviews.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class ReviewConverter
    {
        #region Public Methods and Operators

        public static foundation.Review ToFoundationModel(this webModel.Review review)
        {
            var retVal = new foundation.Review();
            //TODO
            return retVal;
        }

        public static webModel.Review ToWebModel(this foundation.Review review)
        {
            var retVal = new webModel.Review();

            retVal.InjectFrom(review);
            retVal.Id = review.ReviewId;

            retVal.Rating = review.OverallRating;
            retVal.RatingComment = review.Title;
            retVal.ReviewText = review.ReviewFieldValues.Select(x => x.Value).FirstOrDefault();
            if (review.ReviewComments != null)
            {
                retVal.Comments = review.ReviewComments.Select(x => x.ToWebModel()).ToList();
            }
            return retVal;
        }

        #endregion
    }
}
