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
	[EntitySet("CommunicationItems")]
	[DataServiceKey("CommunicationItemId")]
    [KnownType(typeof(PhoneCallItem))]
    [KnownType(typeof(EmailItem))]
    [KnownType(typeof(PublicReplyItem))]
	public abstract class CommunicationItem : StorageEntity
	{

		public CommunicationItem()
		{
			_CommunicationItemId = GenerateNewKey();
		}


		private string _CommunicationItemId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string CommunicationItemId
		{
			get
			{
				return _CommunicationItemId;
			}
			set
			{
				SetValue(ref _CommunicationItemId, () => this.CommunicationItemId, value);
			}
		}


		private string _AuthorName;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string AuthorName
		{
			get
			{
				return _AuthorName;
			}
			set
			{
				SetValue(ref _AuthorName, () => this.AuthorName, value);
			}
		}


		private string _ModifierId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string ModifierId
		{
			get
			{
				return _ModifierId;
			}
			set
			{
				SetValue(ref _ModifierId, () => this.ModifierId, value);
			}
		}


		private string _ModifierName;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string ModifierName
		{
			get
			{
				return _ModifierName;
			}
			set
			{
				SetValue(ref _ModifierName, () => this.ModifierName, value);
			}
		}


		private string _Title;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string Title
		{
			get
			{
				return _Title;
			}
			set
			{
				SetValue(ref _Title, () => this.Title, value);
			}
		}


		private string _Body;
		[DataMember]
		public string Body
		{
			get
			{
				return _Body;
			}
			set
			{
				SetValue(ref _Body, () => this.Body, value);
			}
		}


		#region Navigation Properties

		private string _CaseId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string CaseId
		{
			get { return _CaseId; }
			set
			{
				SetValue(ref _CaseId, () => this.CaseId, value);
			}
		}

		[DataMember]
        [ForeignKey("CaseId")]
        [Parent]
		public virtual Case Case { get; set; }


		private ObservableCollection<Attachment> _Attachments = null;
		[DataMember]
		public ObservableCollection<Attachment> Attachments
		{
			get
			{
				if (_Attachments == null)
					_Attachments = new ObservableCollection<Attachment>();

				return _Attachments;
			}
		}

		#endregion

	}
}
