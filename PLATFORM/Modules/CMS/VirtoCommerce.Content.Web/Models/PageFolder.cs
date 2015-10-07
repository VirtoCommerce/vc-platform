using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	public class PageFolder
	{
		/// <summary>
		/// Page folder name, by-default 'pages' and 'includes'
		/// </summary>
		public string FolderName { get; set; }

		private Collection<Page> _pages;

        /// <summary>
        /// Collection of page elements in this folder
        /// </summary>
		public Collection<Page> Pages { get { return _pages ?? (_pages = new Collection<Page>()); } }

        private Collection<PageFolder> _folders;

        /// <summary>
        /// Collection of folders
        /// </summary>
        public Collection<PageFolder> Folders { get { return _folders ?? (_folders = new Collection<PageFolder>()); } }

        public string[] SecurityScopes { get; set; }
    }
}