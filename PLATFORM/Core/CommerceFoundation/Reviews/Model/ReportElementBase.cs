using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;


namespace VirtoCommerce.Foundation.Reviews.Model
{
	[DataContract]
    [DataServiceKey("ReportElementId")]
	public abstract class ReportElementBase : StorageEntity
	{

        public ReportElementBase()
        {
            ReportElementId = GenerateNewKey();
        }

        private string _ReportElementId;
        [Key]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [DataMember]
        public string ReportElementId
        {
            get
            {
                return _ReportElementId;
            }
            set
            {
                SetValue(ref _ReportElementId, () => this.ReportElementId, value);
            }
        }

		private string _ReviewId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string ReviewId
		{
			get
			{
				return _ReviewId;
			}
			set
			{
				SetValue(ref _ReviewId, () => this.ReviewId, value);
			}
		}

		private string _CommentId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string CommentId
		{
			get
			{
				return _CommentId;
			}
			set
			{
				SetValue(ref _CommentId, () => this.CommentId, value);
			}
		}


		private string _AuthorId;
		[Required]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string AuthorId
		{
			get
			{
				return _AuthorId;
			}
			set
			{
				SetValue(ref _AuthorId, () => this.AuthorId, value);
			}
		}

        [DataMember]
        public Review Review
        {
            get;
            set;
        }
	}
}
