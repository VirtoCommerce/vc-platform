using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using System.Data.Services.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[DataServiceKey("EditorialReviewId")]
	[EntitySet("EditorialReviews")]
	public class EditorialReview : StorageEntity
	{
		public EditorialReview()
		{
			EditorialReviewId = GenerateNewKey();
		}

		private string _EditorialReviewId;
		[Key]
		[StringLength(128)]
		[DataMember]
		public string EditorialReviewId
		{
			get
			{
				return _EditorialReviewId;
			}
			set
			{
				SetValue(ref _EditorialReviewId, () => this.EditorialReviewId, value);
			}
		}

		private int _Priority;
		[DataMember]
		public int Priority
		{
			get
			{
				return _Priority;
			}
			set
			{
				SetValue(ref _Priority, () => this.Priority, value);
			}
		}

		private string _Source;
		[DataMember]
		[StringLength(128)]
		public string Source
		{
			get
			{
				return _Source;
			}
			set
			{
				SetValue(ref _Source, () => this.Source, value);
			}
		}

		private string _Content;
		[DataMember]
		//[StringLength(1024)]
		public string Content
		{
			get
			{
				return _Content;
			}
			set
			{
				SetValue(ref _Content, () => this.Content, value);
			}
		}

		private int _ReviewState;
		[DataMember]
		[Required]
		public int ReviewState
		{
			get
			{
				return _ReviewState;
			}
			set
			{
				SetValue(ref _ReviewState, () => this.ReviewState, value);
			}
		}

		private string _Comments;
		[DataMember]
		//[StringLength(1024)]
		public string Comments
		{
			get
			{
				return _Comments;
			}
			set
			{
				SetValue(ref _Comments, () => this.Comments, value);
			}
		}

		private string _Locale;
		[StringLength(64)]
		[DataMember]
		public string Locale
		{
			get
			{
				return _Locale;
			}
			set
			{
				SetValue(ref _Locale, () => this.Locale, value);
			}
		}

		#region Navigation Properties

		private string _ItemId;
		[DataMember]
		[StringLength(128)]
		[ForeignKey("CatalogItem")]
		[Required]
		public string ItemId
		{
			get
			{
				return _ItemId;
			}
			set
			{
				SetValue(ref _ItemId, () => this.ItemId, value);
			}
		}

		[DataMember]
		[Parent]
		[ForeignKey("ItemId")]
		public Item CatalogItem { get; set; }
		#endregion
	}
}
