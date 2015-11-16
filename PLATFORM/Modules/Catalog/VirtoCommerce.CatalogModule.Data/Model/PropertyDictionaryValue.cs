using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Data.Model
{
    public class PropertyDictionaryValue : Entity
    {
        [StringLength(64)]
        public string Alias { get; set; }

        [StringLength(64)]
        public string Name { get; set; }

        [StringLength(512)]
        public string Value { get; set; }


        [StringLength(64)]
        public string Locale { get; set; }


        #region Navigation Properties
        public string PropertyId { get; set; }
        public virtual Property Property { get; set; }
        #endregion
      
    }
}
