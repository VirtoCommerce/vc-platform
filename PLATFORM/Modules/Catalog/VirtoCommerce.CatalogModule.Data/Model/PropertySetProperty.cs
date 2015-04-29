using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CatalogModule.Data.Model
{
    public class PropertySetProperty : Entity
    {
		public int Priority { get; set; }

        #region Navigation Properties

		[StringLength(128)]
		[Required]
		public string PropertyId { get; set; }

        public virtual Property Property { get; set; }

        [StringLength(128)]
        [Required]
		public string PropertySetId { get; set; }
        [Parent]
        [ForeignKey("PropertySetId")]
        public virtual PropertySet PropertySet { get; set; }
        #endregion
    }
}
