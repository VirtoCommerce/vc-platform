using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Services.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
    [EntitySet("CategoryBases")]
    public class LinkedCategory : CategoryBase
    {
		#region Navigation Properties
		private string _LinkedCatalogId;
		[DataMember]
		[StringLength(128)]
        [ForeignKey("CatalogLink")]
		[Required]
		public string LinkedCatalogId
		{
			get
			{
				return _LinkedCatalogId;
			}
			set
			{
				SetValue(ref _LinkedCatalogId, () => this.LinkedCatalogId, value);
			}
		}

		[DataMember]
		public virtual CatalogBase CatalogLink { get; set; } 

		private string _LinkedCategoryId;
		[DataMember]
		[StringLength(128)]
        [ForeignKey("CategoryLink")]
		public string LinkedCategoryId
		{
			get
			{
				return _LinkedCategoryId;
			}
			set
			{
				SetValue(ref _LinkedCategoryId, () => this.LinkedCategoryId, value);
			}
		}

		[DataMember]
		public virtual CategoryBase CategoryLink { get; set; }
		#endregion

    }
}
