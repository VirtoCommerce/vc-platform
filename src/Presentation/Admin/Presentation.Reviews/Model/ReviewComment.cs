using System.ComponentModel.DataAnnotations;
using Omu.ValueInjecter;
using model = VirtoCommerce.Foundation.Reviews.Model;

namespace VirtoCommerce.ManagementClient.Reviews.Model
{
	public class ReviewComment : ReviewBase
	{
		public ReviewComment(model.ReviewComment item = null) :
			base(item)
		{
			if (item != null)
			{
				this.InjectFrom(item);
				Body = item.Comment;
				Title = item.Review.Title;
				ItemName = item.Review.ItemId;
				ReviewType = ReviewType.Comment;
			}
		}

		private string _reviewId;
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string ReviewId
		{
			get
			{
				return _reviewId;
			}
			set
			{
				_reviewId = value;
				OnPropertyChanged();
			}
		}

		private string _comment;
		public string Comment
		{
			get
			{
				return _comment;
			}
			set
			{
				_comment = value;
				OnPropertyChanged();
			}
		}

	}
}
