using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	public class GetPagesResponse
	{
		private Collection<PageFolder> _folders;
		public Collection<PageFolder> Folders { get { return _folders ?? (_folders = new Collection<PageFolder>()); } }

		private Collection<Page> _pages;
		public Collection<Page> Pages { get { return _pages ?? (_pages = new Collection<Page>()); } }
	}
}