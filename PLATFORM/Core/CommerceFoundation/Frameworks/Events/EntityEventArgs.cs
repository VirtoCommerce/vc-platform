using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Frameworks.Events
{
    public class EntityEventArgs : EventArgs
    {
        public IPropertyValues CurrentValues { get; private set; }
        public IPropertyValues OriginalValues { get; private set; }

        public EntityState State { get; private set; }

        public EntityEventArgs(EntityState state, IPropertyValues originalValues, IPropertyValues currentValues)
        {
            State = state;
            CurrentValues = currentValues;
            OriginalValues = originalValues;
        }
    }
}
