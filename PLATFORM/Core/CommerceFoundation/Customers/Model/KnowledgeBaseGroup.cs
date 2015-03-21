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
	[EntitySet("KnowledgeBaseGroups")]
	[DataServiceKey("KnowledgeBaseGroupId")]
	public class KnowledgeBaseGroup : StorageEntity
	{

		public KnowledgeBaseGroup()
		{
			_KnowledgeBaseGroupId = GenerateNewKey();
		}


		private string _KnowledgeBaseGroupId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string KnowledgeBaseGroupId
		{
			get
			{
				return _KnowledgeBaseGroupId;
			}
			set
			{
				SetValue(ref _KnowledgeBaseGroupId, () => this.KnowledgeBaseGroupId, value);
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


		private string _Name;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				SetValue(ref _Name, () => this.Name, value);
			}
		}


		#region Navigation Properties

		private string _ParentId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string ParentId
		{
			get
			{
				return _ParentId;
			}
			set
			{
				SetValue(ref _ParentId, () => this.ParentId, value);
			}
		}

		[DataMember]
        [ForeignKey("ParentId")]
		public virtual KnowledgeBaseGroup Parent { get; set; }


		private ObservableCollection<KnowledgeBaseArticle> _KnowledgeBaseArticles;
		[DataMember]
		public ObservableCollection<KnowledgeBaseArticle> KnowledgeBaseArticles
		{
			get
			{
				if (_KnowledgeBaseArticles == null)
					_KnowledgeBaseArticles = new ObservableCollection<KnowledgeBaseArticle>();

				return _KnowledgeBaseArticles;
			}
		}

		#endregion

	}
}
