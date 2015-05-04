using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	public class PageFolder
	{
		public string FolderName { get; set; }

		private Collection<Page> _pages;
		public Collection<Page> Pages { get { return _pages ?? (_pages = new Collection<Page>()); } }
	}
}