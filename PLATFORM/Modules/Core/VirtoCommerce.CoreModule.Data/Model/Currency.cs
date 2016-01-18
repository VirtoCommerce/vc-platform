using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CoreModule.Data.Model
{
    public class Currency : AuditableEntity
    {
        [Required]
        [StringLength(16)]
        [Index("IX_Code")]
        public string Code { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public bool IsPrimary { get; set; }

        [Column(TypeName = "Money")]
        public decimal ExchangeRate { get; set; }

        [StringLength(16)]
        public string Symbol { get; set; }

        [StringLength(64)]
        public string CustomFormatting { get; set; }

    }
}
