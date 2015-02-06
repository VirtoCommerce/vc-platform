using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;
using foundation = VirtoCommerce.Foundation.Reviews.Model;
namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
	public static class ReviewCommentConverter
	{
		public static webModel.ReviewComment ToWebModel(this foundation.ReviewComment comment)
		{
			var retVal = new webModel.ReviewComment();

			retVal.InjectFrom(comment);
			retVal.Id = comment.ReviewCommentId;
			retVal.Author = comment.AuthorName;

			return retVal;
		}

		public static foundation.ReviewComment ToFoundationModel(this webModel.ReviewComment comment)
		{
			var retVal = new foundation.ReviewComment();
			//TODO
			return retVal;
		}


	}
}
