using Omu.ValueInjecter;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;
using foundation = VirtoCommerce.Foundation.Reviews.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class ReviewCommentConverter
    {
        #region Public Methods and Operators

        public static foundation.ReviewComment ToFoundationModel(this webModel.ReviewComment comment)
        {
            var retVal = new foundation.ReviewComment();
            //TODO
            return retVal;
        }

        public static webModel.ReviewComment ToWebModel(this foundation.ReviewComment comment)
        {
            var retVal = new webModel.ReviewComment();

            retVal.InjectFrom(comment);
            retVal.Id = comment.ReviewCommentId;
            retVal.Author = comment.AuthorName;

            return retVal;
        }

        #endregion
    }
}
