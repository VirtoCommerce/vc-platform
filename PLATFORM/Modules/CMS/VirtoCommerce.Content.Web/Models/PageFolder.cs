using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	/// <summary>
	/// Page folder
	/// </summary>
	public class PageFolder
	{
		/// <summary>
		/// Page folder name
		/// </summary>
		public string FolderName { get; set; }

		private Collection<Page> _pages;

		/// <summary>
		/// Collection of pages
		/// </summary>
		public Collection<Page> Pages { get { return _pages ?? (_pages = new Collection<Page>()); } }
	}
}