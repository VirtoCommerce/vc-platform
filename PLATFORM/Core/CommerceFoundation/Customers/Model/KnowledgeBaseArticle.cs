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
	[EntitySet("KnowledgeBaseArticles")]
	[DataServiceKey("KnowledgeBaseArticleId")]
	public class KnowledgeBaseArticle : StorageEntity
	{

		public KnowledgeBaseArticle()
		{
			_KnowledgeBaseArticleId = GenerateNewKey();
		}


		private string _KnowledgeBaseArticleId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string KnowledgeBaseArticleId
		{
			get
			{
				return _KnowledgeBaseArticleId;
			}
			set
			{
				SetValue(ref _KnowledgeBaseArticleId, () => this.KnowledgeBaseArticleId, value);
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

        private string _AuthorId;
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

		private ObservableCollection<Attachment> _Attachments;
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

        private string _GroupId;
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string GroupId
        {
            get
            {
                return _GroupId;
            }
            set
            {
                SetValue(ref _GroupId, () => this.GroupId, value);
            }
        }

        [DataMember]
        [ForeignKey("GroupId")]
        public virtual KnowledgeBaseGroup Group { get; set; }

        #endregion

	}
}
