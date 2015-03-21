using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

using System.Data.Services.Common;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Customers.Model
{
	[DataContract]
	[EntitySet("Attachments")]
	[DataServiceKey("AttachmentId")]
	public class Attachment : StorageEntity
	{

		public Attachment()
		{
			_AttachmentId = GenerateNewKey();
		}

		private string _AttachmentId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string AttachmentId
		{
			get { return _AttachmentId; }
			set
			{
				SetValue(ref _AttachmentId, () => this.AttachmentId, value);
			}

		}


		private string _CreatorName;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string CreatorName
		{
			get { return _CreatorName; }
			set
			{
				SetValue(ref _CreatorName, () => this.CreatorName, value);
			}

		}


		private string _FileUrl;
		[DataMember]
		[StringLength(512, ErrorMessage = "Only 512 characters allowed.")]
		public string FileUrl
		{
			get { return _FileUrl; }
			set
			{
				SetValue(ref _FileUrl, () => this.FileUrl, value);
			}

		}


		private string _DisplayName;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string DisplayName
		{
			get { return _DisplayName; }
			set
			{
				SetValue(ref _DisplayName, () => this.DisplayName, value);
			}

		}


		private string _FileType;
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string FileType
		{

			get { return _FileType; }
			set { SetValue(ref _FileType, () => this.FileType, value); }
		}


		#region Navigation Properties

		private string _CommunicationItemId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string CommunicationItemId
		{
			get { return _CommunicationItemId; }
			set
			{
				SetValue(ref _CommunicationItemId, () => this.CommunicationItemId, value);
			}
		}

		[DataMember]
        [ForeignKey("CommunicationItemId")]
		public virtual CommunicationItem CommunicationItem { get; set; }


		private string _KnowledgeBaseArticleId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string KnowledgeBaseArticleId
		{
			get { return _KnowledgeBaseArticleId; }
			set
			{
				SetValue(ref _KnowledgeBaseArticleId, () => this.KnowledgeBaseArticleId, value);
			}
		}

		[DataMember]
        [ForeignKey("KnowledgeBaseArticleId")]
		public virtual KnowledgeBaseArticle KnowledgeBaseArticle { get; set; }

		#endregion
	}
}
