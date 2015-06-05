using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CatalogModule.Data.Model
{
	public class PropertySet : AuditableEntity
	{
		public PropertySet()
		{
			Id = Guid.NewGuid().ToString("N");
			PropertySetProperties = new NullCollection<PropertySetProperty>();
		}
		[Required]
		[StringLength(128)]
		public string Name { get; set; }

		[Required]
		[StringLength(64)]
		public string TargetType { get; set; }

		#region NavigationProperties
	
		public virtual ObservableCollection<PropertySetProperty> PropertySetProperties { get; set; }

		#endregion
	}
}
