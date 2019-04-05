﻿using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.ChangeLog
{
    public class OperationLog : AuditableEntity, ICloneable
    {
        public string ObjectType { get; set; }

        public string ObjectId { get; set; }

        public EntryState OperationType { get; set; }

        public string Detail { get; set; }
        
        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}
