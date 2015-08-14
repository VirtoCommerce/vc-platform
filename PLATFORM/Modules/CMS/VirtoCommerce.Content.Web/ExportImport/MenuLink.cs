using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.ExportImport
{
    public class MenuLink
    {
        public string Id { get; set; }

        /// <summary>
        /// Title of menu link element, displayed as link text or link title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Url of menu link element, inserts in href attribute of link
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Priority of menu link element, the higher the value, the higher in the list
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// If true - will displayed in the list, if false - not
        /// </summary>
        public bool IsActive { get; set; }

        public string MenuLinkListId { get; set; }
    }
}