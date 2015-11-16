using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Data.Model
{
	public class Catalog : AuditableEntity
	{
		public Catalog()
		{
	
            CatalogLanguages = new NullCollection<CatalogLanguage>();
            CatalogPropertyValues = new NullCollection<PropertyValue>();
            IncommingLinks = new NullCollection<CategoryRelation>();
            Properties = new NullCollection<Property>();
        }

        public bool Virtual { get; set; }
		[Required]
		[StringLength(128)]
		public string Name { get; set; }

		[StringLength(64)]
		[Required]
		public string DefaultLanguage { get; set; }

		[StringLength(128)]
		public string OwnerId { get; set; }

		#region Navigation Properties
		public virtual ObservableCollection<CategoryRelation> IncommingLinks{ get; set; }

        public virtual ObservableCollection<CatalogLanguage> CatalogLanguages { get; set; }
        public virtual ObservableCollection<PropertyValue> CatalogPropertyValues { get; set; }
        public virtual ObservableCollection<Property> Properties { get; set; }
        #endregion

    }
}
