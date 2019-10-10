using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class OperationLogEntity : AuditableEntity
    {

        [StringLength(50)]
        public string ObjectType { get; set; }

        [StringLength(200)]
        public string ObjectId { get; set; }

        [Required]
        [StringLength(20)]
        public string OperationType { get; set; }


        [StringLength(1024)]
        public string Detail { get; set; }

        public virtual OperationLog ToModel(OperationLog operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            operation.CreatedBy = CreatedBy;
            operation.CreatedDate = CreatedDate;
            operation.Detail = Detail;
            operation.Id = Id;
            operation.ModifiedBy = ModifiedBy;
            operation.ModifiedDate = ModifiedDate;
            operation.ObjectId = ObjectId;
            operation.ObjectType = ObjectType;
            operation.OperationType = EnumUtility.SafeParse(OperationType, EntryState.Unchanged);
            return operation;
        }

        public virtual OperationLogEntity FromModel(OperationLog operation, PrimaryKeyResolvingMap pkMap)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            pkMap.AddPair(operation, this);

            CreatedBy = operation.CreatedBy;
            CreatedDate = operation.CreatedDate;
            Detail = operation.Detail;
            Id = operation.Id;
            ModifiedBy = operation.ModifiedBy;
            ModifiedDate = operation.ModifiedDate;
            ObjectId = operation.ObjectId;
            ObjectType = operation.ObjectType;
            OperationType = operation.OperationType.ToString();

            return this;
        }

        public virtual void Patch(OperationLogEntity target)
        {
            target.Detail = Detail;
        }
    }

}
