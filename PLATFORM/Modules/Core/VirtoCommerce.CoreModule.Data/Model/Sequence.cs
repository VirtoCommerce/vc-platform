using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CoreModule.Data.Model
{
    public class Sequence 
    {
        [Key]
        [StringLength(256)]
        public string ObjectType { get; set; }

        [Required]
        public int Value { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
