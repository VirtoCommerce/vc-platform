using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Services.Common;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Foundation.Frameworks.Logging
{
    [DataContract]
    [EntitySet("__OperationLogs")]
    [DataServiceKey("OperationLogId")]
    public class OperationLog
    {
        public OperationLog()
        {
            OperationLogId = Guid.NewGuid().ToString();
        }

        [Key]
        [DataMember]
        public string OperationLogId
        {
            get;
            set;
        }

        [Required]
        [StringLength(50)]
        [DataMember]
        public string ModifiedBy
        {
            get;
            set;
        }

        [Required]
        [StringLength(50)]
        [DataMember]
        public string ObjectType
        {
            get;set;
        }

        [Required]
        [StringLength(200)]
        [DataMember]
        public string ObjectId
        {
            get;
            set;
        }

        [Required]
        [StringLength(200)]
        [DataMember]
        public string TableName
        {
            get;
            set;
        }

        [Required]
        [StringLength(20)]
        [DataMember]
        public string OperationType
        {
            get;
            set;
        }

        [DataMember]
        [Required]
        public DateTime LastModified
        {
            get;
            set;
        }
    }
}
