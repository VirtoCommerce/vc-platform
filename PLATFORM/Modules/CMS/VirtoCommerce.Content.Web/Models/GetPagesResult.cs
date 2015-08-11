using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	public class GetPagesResult
	{
		private Collection<PageFolder> _folders;

		/// <summary>
		/// Collection of pages folders (by default - 'pages', 'includes'), that contains page elements
		/// </summary>
		public Collection<PageFolder> Folders { get { return _folders ?? (_folders = new Collection<PageFolder>()); } }

		private Collection<Page> _pages;

		/// <summary>
		/// Collection of page elements (used in pages rendering (page html, images, etc.))
		/// </summary>
		public Collection<Page> Pages { get { return _pages ?? (_pages = new Collection<Page>()); } }
	}
}