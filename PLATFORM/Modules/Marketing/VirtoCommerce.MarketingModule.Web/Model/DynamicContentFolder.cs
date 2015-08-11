using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Web.Model
{
	/// <summary>
	/// Represent folder contains dynamic content system entries, used for hierarchy storing and easy management 
	/// </summary>
	public class DynamicContentFolder : AuditableEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		/// <summary>
		/// Folder all parent ids concatenated (1;21;344)
		/// </summary>
		public string Outline { get; set; }
		/// <summary>
		/// Represent folder path with folder names in hierarchy (Root\Child\Child2)
		/// </summary>
		public string Path { get; set; }
		public string ParentFolderId { get; set; }
		public string ImageUrl { get; set; }
		
	}
}