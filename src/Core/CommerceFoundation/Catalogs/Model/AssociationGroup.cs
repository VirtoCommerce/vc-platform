using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[DataServiceKey("AssociationGroupId")]
	[EntitySet("AssociationGroups")]
	public class AssociationGroup : StorageEntity
	{
		public AssociationGroup()
		{
			_AssociationGroupId = GenerateNewKey();
		}

		private string _AssociationGroupId;
		[Key]
		[StringLength(128)]
		[DataMember]
		public string AssociationGroupId
		{
			get
			{
				return _AssociationGroupId;
			}
			set
			{
				SetValue(ref _AssociationGroupId, () => this.AssociationGroupId, value);
			}
		}

		private string _Name;
		[StringLength(128)]
		[Required]
		[DataMember]
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

		private string _Description;
		[StringLength(512)]
		[DataMember]
		public string Description
		{
			get
			{
				return _Description;
			}
			set
			{
				SetValue(ref _Description, () => this.Description, value);
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

		#region Navigation Properties

		private string _ItemId;
		[DataMember]
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
		public virtual Item CatalogItem { get; set; }

		ObservableCollection<Association> _Associations;
		[DataMember]
		public virtual ObservableCollection<Association> Associations
		{
			get
			{
				if (_Associations == null)
					_Associations = new ObservableCollection<Association>();

				return _Associations;
			}
			set { _Associations = value; }
		}

		#endregion
	}
}
