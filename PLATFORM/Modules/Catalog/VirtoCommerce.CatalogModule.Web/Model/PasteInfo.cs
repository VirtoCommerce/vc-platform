using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    /// <summary>
    /// Represent move operation detail
    /// </summary>
    public class MoveInfo
    {
        public string Catalog { get; set; }
        public string Category { get; set; }

        public ICollection<ListEntry> ListEntries { get; set; }
    }
}