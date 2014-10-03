using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[EntitySet("ItemPropertyValues")]
	public class ItemPropertyValue : PropertyValueBase
	{
		#region Navigation Properties

		private string _ItemId;
		[DataMember]
		[StringLength(128)]
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
		public virtual Item CatalogItem { get; set; }

        private string _PropertyId;
        [DataMember]
        [StringLength(128)]
        public string PropertyId
        {
            get
            {
                return _PropertyId;
            }
            set
            {
                SetValue(ref _PropertyId, () => this.PropertyId, value);
            }
        }

        [DataMember]
        [Parent]
        [ForeignKey("PropertyId")]
        public virtual Property Property { get; set; }
		#endregion
	}
}
