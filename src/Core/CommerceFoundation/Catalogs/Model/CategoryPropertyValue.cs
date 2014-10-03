using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[EntitySet("CategoryPropertyValues")]
	public class CategoryPropertyValue : PropertyValueBase
	{
		#region Navigation Properties
		private string _CategoryId;
		[DataMember]
		[StringLength(128)]
		[Required]
		public string CategoryId
		{
			get
			{
				return _CategoryId;
			}
			set
			{
				SetValue(ref _CategoryId, () => this.CategoryId, value);
			}
		}

		[DataMember]
		[Parent]
		[ForeignKey("CategoryId")]
		public virtual Category Category { get; set; }

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
