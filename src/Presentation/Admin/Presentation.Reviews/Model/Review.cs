using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using Omu.ValueInjecter;
using model = VirtoCommerce.Foundation.Reviews.Model;

namespace VirtoCommerce.ManagementClient.Reviews.Model
{
	public class Review : ReviewBase
	{
		public Review(model.Review item = null) :
			base(item)
		{
			if (item != null)
			{
				this.InjectFrom(item);
				Body = item.ReviewFieldValues.First(value => value.Name == "Review").Value;
				ItemName = item.ItemId;
				ReviewType = ReviewType.Review;
			}
		}


		private string _itemUrl;
		[StringLength(1024, ErrorMessage = "Only 1024 characters allowed.")]
		public string ItemUrl
		{
			get
			{
				return _itemUrl;
			}
			set
			{
				_itemUrl = value;
				OnPropertyChanged();
			}
		}

		private string _itemId;
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string ItemId
		{
			get
			{
				return _itemId;
			}
			set
			{
				_itemId = value;
				OnPropertyChanged();
			}
		}

		#region Navigation properties

		ObservableCollection<model.ReviewComment> _comments;
		public ObservableCollection<model.ReviewComment> ReviewComments
		{
			get { return _comments ?? (_comments = new ObservableCollection<model.ReviewComment>()); }
		}

		#endregion
	}
}
