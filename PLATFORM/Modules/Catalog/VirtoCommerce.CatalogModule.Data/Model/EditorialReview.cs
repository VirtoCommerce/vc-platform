﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CatalogModule.Data.Model
{
	public class EditorialReview : AuditableEntity
	{
	
		public int Priority { get; set; }

		[StringLength(128)]
		public string Source { get; set; }

		public string Content { get; set; }

		[Required]
		public int ReviewState { get; set; }

		public string Comments { get; set; }

		[StringLength(64)]
		public string Locale { get; set; }

		#region Navigation Properties
		public string ItemId { get; set; }

		public Item CatalogItem { get; set; }
		#endregion
	}
}
