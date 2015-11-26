﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Data.Model
{
	public class Property : AuditableEntity
	{
		public Property()
		{
			DictionaryValues = new NullCollection<PropertyDictionaryValue>();
			PropertyAttributes = new NullCollection<PropertyAttribute>();
		}

		[Required]
		[StringLength(128)]
		public string Name { get; set; }

		[StringLength(128)]
		public string TargetType { get; set; }

		public bool IsKey { get; set; }

		public bool IsSale { get; set; }

		public bool IsEnum { get; set; }

		public bool IsInput { get; set; }

		public bool IsRequired { get; set; }

		public bool IsMultiValue { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is locale dependant. If true, the locale must be specified for the values.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is locale dependant; otherwise, <c>false</c>.
		/// </value>
		public bool IsLocaleDependant { get; set; }

		public bool AllowAlias { get; set; }

		[Required]
		public int PropertyValueType { get; set; }

		#region Navigation Properties

		public string CatalogId { get; set; }
		public virtual Catalog Catalog { get; set; }

        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ObservableCollection<PropertyDictionaryValue> DictionaryValues { get; set; }
		public virtual ObservableCollection<PropertyAttribute> PropertyAttributes { get; set; }

		#endregion
	}
}
