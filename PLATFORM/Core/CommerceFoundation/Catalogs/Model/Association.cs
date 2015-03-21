using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[DataServiceKey("AssociationId")]
	[EntitySet("Associations")]
	public class Association : StorageEntity
	{
		public Association()
		{
			_AssociationId = GenerateNewKey();
		}

		private string _AssociationId;
		[Key]
		[StringLength(128)]
		[DataMember]
		public string AssociationId
		{
			get
			{
				return _AssociationId;
			}
			set
			{
				SetValue(ref _AssociationId, () => this.AssociationId, value);
			}
		}

		private string _AssociationType;
		/// <summary>
		/// Gets or sets the type of the association. The examples association types are: optional, required. AssociationTypes.Required or AssociationTypes.Optional
		/// </summary>
		/// <value>
		/// The type of the association.
		/// </value>
		[StringLength(128)]
		[Required(ErrorMessage = "Field 'Association type' is required.")]
		[DataMember]
		public string AssociationType
		{
			get
			{
				return _AssociationType;
			}
			set
			{
				SetValue(ref _AssociationType, () => this.AssociationType, value);
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

		private string _AssociationGroupId;
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

		[DataMember]
		[Parent]
		[ForeignKey("AssociationGroupId")]
		public virtual AssociationGroup AssociationGroup { get; set; }

		private string _ItemId;
		[DataMember]
		[StringLength(128)]
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
		[ForeignKey("ItemId")]
		public virtual Item CatalogItem { get; set; }
		#endregion
	}
}
