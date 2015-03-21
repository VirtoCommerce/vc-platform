using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Foundation.Frameworks.Logging
{
    public partial class AuditTrail
    {
        [Key]
        public string AuditTrailId {get;set;}
        
        [Required]
        public DateTime Updated {get;set;}
        
        [StringLength(255)]
        public string ChangedBy {get;set;}
        
        [StringLength(1024)]
        public string OldValues {get;set;}
        
        [StringLength(1024)]
        public string NewValues {get;set;}
        
        [StringLength(255)]
        public string ChangeType { get; set; }

        [StringLength(255)]
        [Required]
        public string KeySegment { get; set; }

        [StringLength(255)]
        [Required]
        public string DataSource { get; set; }
    }
}
