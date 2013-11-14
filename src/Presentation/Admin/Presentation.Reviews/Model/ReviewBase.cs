using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using Omu.ValueInjecter;
using model = VirtoCommerce.Foundation.Reviews.Model;

namespace VirtoCommerce.ManagementClient.Reviews.Model
{
	public class ReviewBase : NotifyPropertyChanged
	{
		public ReviewBase(object item = null)
		{
			if (item != null)
			{
				this.InjectFrom(item);

				var propInfo = item.GetType().FindPropertiesWithAttribute(typeof(KeyAttribute)).First();
				ReviewBaseId = propInfo.GetValue(item) as string ?? Guid.NewGuid().ToString();
			}
		}

		private string _reviewbaseId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string ReviewBaseId
		{
			get
			{
				return _reviewbaseId;
			}
			set
			{
				_reviewbaseId = value;
				OnPropertyChanged();
			}
		}

		private ReviewType _reviewType;
		public ReviewType ReviewType
		{
			get
			{
				return _reviewType;
			}
			set
			{
				_reviewType = value;
				OnPropertyChanged();
			}
		}

		private string _itemName;
		public string ItemName
		{
			get
			{
				return _itemName;
			}
			set
			{
				_itemName = value;
				OnPropertyChanged();
			}
		}

		private string _authorName;
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string AuthorName
		{
			get
			{
				return _authorName;
			}
			set
			{
				_authorName = value;
				OnPropertyChanged();
			}
		}

		private bool _setStatus;
		public bool SetStatus
		{
			get
			{
				return _setStatus;
			}
			set
			{
				_setStatus = value;
				OnPropertyChanged();
			}
		}

		private string _authorLocation;
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string AuthorLocation
		{
			get
			{
				return _authorLocation;
			}
			set
			{
				_authorLocation = value;
				OnPropertyChanged();
			}
		}

		private string _body;
		public string Body
		{
			get
			{
				return _body;
			}
			set
			{
				_body = value;
				OnPropertyChanged();
			}
		}

		private int _overallRating;
		public int OverallRating
		{
			get
			{
				return _overallRating;
			}
			set
			{
				_overallRating = value;
				OnPropertyChanged();
			}
		}

		private int _totalAbuseCount;
		public int TotalAbuseCount
		{
			get
			{
				return _totalAbuseCount;
			}
			set
			{
				_totalAbuseCount = value;
				OnPropertyChanged();
			}
		}

		private int _totalPositiveFeedbackCount;
		public int TotalPositiveFeedbackCount
		{
			get
			{
				return _totalPositiveFeedbackCount;
			}
			set
			{
				_totalPositiveFeedbackCount = value;
				OnPropertyChanged();
			}
		}

		private int _totalNegativeFeedbackCount;
		public int TotalNegativeFeedbackCount
		{
			get
			{
				return _totalNegativeFeedbackCount;
			}
			set
			{
				_totalNegativeFeedbackCount = value;
				OnPropertyChanged();
			}
		}

		private int _priority;
		public int Priority
		{
			get
			{
				return _priority;
			}
			set
			{
				_priority = value;
				OnPropertyChanged();
			}
		}

		private int _status;
		public int Status
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
				OnPropertyChanged();
			}
		} // Pending, Approved, Denied

		private DateTime? _created;
		public DateTime? Created
		{
			get
			{
				return _created;
			}
			set
			{
				_created = value;
				OnPropertyChanged();
			}
		}

		private string _contentLocale;
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string ContentLocale
		{
			get
			{
				return _contentLocale;
			}
			set
			{
				_contentLocale = value;
				OnPropertyChanged();
			}
		} // en-us (for example)

		private string _title;
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				_title = value;
				OnPropertyChanged();
			}
		}
	}
}
