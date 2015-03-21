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

namespace VirtoCommerce.Foundation.Customers.Model
{
	[DataContract]
	[EntitySet("Notes")]
	[DataServiceKey("NoteId")]
	public class Note : StorageEntity
	{

		public Note()
		{
			_NoteId = GenerateNewKey();
		}


		private string _NoteId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string NoteId
		{
			get
			{
				return _NoteId;
			}
			set
			{
				SetValue(ref _NoteId, () => this.NoteId, value);
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


		private bool _IsSticky;
		[DataMember]
		public bool IsSticky
		{
			get
			{
				return _IsSticky;
			}
			set
			{
				SetValue(ref _IsSticky, () => this.IsSticky, value);
			}
		}



		#region Navigation Properties

		private string _MemberId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string MemberId
		{
			get { return _MemberId; }
			set
			{
                SetValue(ref _MemberId, () => this.MemberId, value);
			}
		}

		[DataMember]
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }



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
		public virtual Case Case { get; set; }
		#endregion


	}
}
