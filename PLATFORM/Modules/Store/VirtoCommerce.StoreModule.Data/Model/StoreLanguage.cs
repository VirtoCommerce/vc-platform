using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.StoreModule.Data.Model
{
    public class StoreLanguage : Entity
    {

		[Required]
		[StringLength(32)]
		public string LanguageCode { get; set; }

        #region Navigation Properties
        [ForeignKey("Store")]
         [Required]
        [StringLength(128)]
		public string StoreId { get; set; }

        public Store Store { get; set; }

        #endregion
    }
}
