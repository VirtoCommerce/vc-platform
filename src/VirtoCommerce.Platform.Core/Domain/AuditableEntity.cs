using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VirtoCommerce.Platform.Core.Common
{
    public abstract class AuditableEntity : Entity, IAuditable
    {
        #region IAuditable Members
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [StringLength(64)]
        public string CreatedBy { get; set; }
        [StringLength(64)]
        public string ModifiedBy { get; set; }
        #endregion

        /// <summary>
        /// Copies what the save generated from this persisted entity to its model: the audit fields by default.
        /// Override to sync more, such as values the database generates.
        /// </summary>
        public virtual void SyncGeneratedValues(IEntity model)
        {
            if (model is IAuditable auditableModel)
            {
                auditableModel.CreatedDate = CreatedDate;
                auditableModel.CreatedBy = CreatedBy;
                auditableModel.ModifiedDate = ModifiedDate;
                auditableModel.ModifiedBy = ModifiedBy;
            }
        }

        #region Conditional JSON serialization for properties declared in base type
        [JsonIgnore]
        public virtual bool ShouldSerializeAuditableProperties { get; } = true;
        /// <summary>
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public virtual bool ShouldSerializeCreatedDate()
        {
            return ShouldSerializeAuditableProperties;
        }
        public virtual bool ShouldSerializeModifiedDate()
        {
            return ShouldSerializeAuditableProperties;
        }
        public virtual bool ShouldSerializeCreatedBy()
        {
            return ShouldSerializeAuditableProperties;
        }
        public virtual bool ShouldSerializeModifiedBy()
        {
            return ShouldSerializeAuditableProperties;
        }
        #endregion
    }
}
