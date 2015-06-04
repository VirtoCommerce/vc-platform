using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.Data.Services.Common;
using System.Collections.ObjectModel;

namespace VirtoCommerce.Foundation.Reviews.Model
{
	[DataContract]
	[EntitySet("Reviews")]
    [DataServiceKey("ReviewId")]
	public class Review : StorageEntity
    {
        public Review()
        {
            ReviewId = GenerateNewKey();
        }

		private string _reviewId;
        [Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
        public string ReviewId
        {
            get
            {
				return _reviewId;
            }
            set
            {
                SetValue(ref _reviewId, () => this.ReviewId, value);
            }
        }

		private string _itemUrl;
        [Required]
		[StringLength(1024, ErrorMessage = "Only 1024 characters allowed.")]
		[DataMember]
        public string ItemUrl
        {
            get
            {
				return _itemUrl;
            }
            set
            {
                SetValue(ref _itemUrl, () => this.ItemUrl, value);
            }
        }

		private string _itemId;
        [Required]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
        public string ItemId
        {
            get
            {
				return _itemId;
            }
            set
            {
                SetValue(ref _itemId, () => this.ItemId, value);
            }
        }
		private int _schemaId;
		[DataMember]
        public int SchemaId
        {
            get
            {
				return _schemaId;
            }
            set
            {
                SetValue(ref _schemaId, () => this.SchemaId, value);
            }
        }

		private string _title;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string Title
        {
            get
            {
				return _title;
            }
            set
            {
                SetValue(ref _title, () => this.Title, value);
            }
        }

		private string _authorId;
        [Required]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
        public string AuthorId
        {
            get
            {
				return _authorId;
            }
            set
            {
                SetValue(ref _authorId, () => this.AuthorId, value);				
            }
        }

		private string _authorName;
        [Required]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
        public string AuthorName
        {
            get
            {
				return _authorName;
            }
            set
            {
                SetValue(ref _authorName, () => this.AuthorName, value);
            }
        }

		private string _authorLocation;
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
        public string AuthorLocation
        {
            get
            {
				return _authorLocation;
            }
            set
            {
                SetValue(ref _authorLocation, () => this.AuthorLocation, value);
            }
        }

		private int _overallRating;
		[DataMember]
        public int OverallRating
        {
            get
            {
				return _overallRating;
            }
            set
            {
                SetValue(ref _overallRating, () => this.OverallRating, value);
            }
        }

		private int _totalAbuseCount;
		[DataMember]
        public int TotalAbuseCount
        {
            get
            {
				return _totalAbuseCount;
            }
            set
            {
                SetValue(ref _totalAbuseCount, () => this.TotalAbuseCount, value);
            }
        }

		private int _totalPositiveFeedbackCount;
		[DataMember]
        public int TotalPositiveFeedbackCount
        {
            get
            {
				return _totalPositiveFeedbackCount;
            }
            set
            {
                SetValue(ref _totalPositiveFeedbackCount, () => this.TotalPositiveFeedbackCount, value);
            }
        }

		private int _totalNegativeFeedbackCount;
		[DataMember]
        public int TotalNegativeFeedbackCount
        {
            get
            {
				return _totalNegativeFeedbackCount;
            }
            set
            {
                SetValue(ref _totalNegativeFeedbackCount, () => this.TotalNegativeFeedbackCount, value);
            }
        }

		private int _priority;
		[DataMember]
        public int Priority
        {
            get
            {
				return _priority;
            }
            set
            {
                SetValue(ref _priority, () => this.Priority, value);
            }
        }

		private int _status;
		[DataMember]
		public int Status
        {
            get
            {
				return _status;
            }
            set
            {
                SetValue(ref _status, () => this.Status, value);
            }
        } // Pending, Approved, Denied	   

		private string _contentLocale;
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		[DataMember]
        public string ContentLocale
        {
            get
            {
				return _contentLocale;
            }
            set
            {
                SetValue(ref _contentLocale, () => this.ContentLocale, value);
            }
        } // en-us (for example)

        private bool _IsVerifiedBuyer;
        [DataMember]
        public bool IsVerifiedBuyer
        {
            get
            {
                return _IsVerifiedBuyer;
            }
            set
            {
                SetValue(ref _IsVerifiedBuyer, () => this.IsVerifiedBuyer, value);
            }
        }

		#region Navigation properties
		// Fields Collection
        ObservableCollection<ReviewFieldValue> _fields = null;
        [DataMember]
		public ObservableCollection<ReviewFieldValue> ReviewFieldValues
        {
            get
            {
				if (_fields == null)
					_fields = new ObservableCollection<ReviewFieldValue>();

				return _fields;
            }
        }

		ObservableCollection<MediaContent> _media = null;
        [DataMember]
		public ObservableCollection<MediaContent> MediaContents
        {
            get
            {
                if (_media == null)
					_media = new ObservableCollection<MediaContent>();

                return _media;
            }
        }

		ObservableCollection<ReviewComment> _comments = null;
        [DataMember]
		public ObservableCollection<ReviewComment> ReviewComments
        {
            get
            {
                if (_comments == null)
					_comments = new ObservableCollection<ReviewComment>();

                return _comments;
            }
		}
		#endregion
	}
}
