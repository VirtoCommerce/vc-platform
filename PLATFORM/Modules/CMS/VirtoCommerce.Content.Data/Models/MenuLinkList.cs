using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Content.Data.Models
{
    public class MenuLinkList : AuditableEntity
    {
        public MenuLinkList()
        {
            MenuLinks = new ObservableCollection<MenuLink>();
        }

        [Required]
        public string Name { get; set; }
        [Required]
        public string StoreId { get; set; }

        [Required]
        public string Language { get; set; }

        public virtual ObservableCollection<MenuLink> MenuLinks { get; set; }

        
    }
}
