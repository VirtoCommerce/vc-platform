using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Foundation.Frameworks;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Reviews.Model
{
    [DataContract]
    [EntitySet("ReviewComments")]
    [DataServiceKey("ReviewCommentId")]
    public class ReviewComment : StorageEntity
    {
        public ReviewComment()
		{
            ReviewCommentId = GenerateNewKey();
		}

        private string _reviewCommentId;
        [Key]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [DataMember]
        public string ReviewCommentId
        {
            get
            {
                return _reviewCommentId;
            }
            set
            {
                _reviewCommentId = value;
                OnPropertyChanged("ReviewCommentId");
            }
        }

        private string _reviewId;
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
                _reviewId = value;
                OnPropertyChanged("ReviewId");
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
                _authorId = value;
                OnPropertyChanged("AuthorId");
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
                _authorName = value;
                OnPropertyChanged("AuthorName");
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
                _authorLocation = value;
                OnPropertyChanged("AuthorLocation");
            }
        }

        private string _comment;
        [DataMember]
        [StringLength(1024, ErrorMessage = "Only 1024 characters allowed.")]
        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
                OnPropertyChanged("Comment");
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
                _status = value;
                OnPropertyChanged("Status");
            }
        } //  Pending, Approved, Denied					 


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
                _totalAbuseCount = value;
                OnPropertyChanged("TotalAbuseCount");
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
                _totalPositiveFeedbackCount = value;
                OnPropertyChanged("TotalPositiveFeedbackCount");
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
                _totalNegativeFeedbackCount = value;
                OnPropertyChanged("TotalNegativeFeedbackCount");
            }
        }

        [DataMember]
        [Parent]
        [ForeignKey("ReviewId")]
        public Review Review
        {
            get;
            set;
        }
    }
}
