using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Frameworks.Events
{
    public abstract class TypeEntityEventListener : IEntityEventListener
    {
        private readonly System.Type _targetType;
        public Type TargetType { get { return _targetType; } }

        protected TypeEntityEventListener(System.Type targetType)
        {
            this._targetType = targetType;
        }

        protected abstract void OnEntityBeforeSaved(object sender, EntityEventArgs e);
        protected abstract void OnEntityAfterSaved(object sender, EntityEventArgs e);

        public virtual bool IsTargetEntity(object item)
        {
            return this.TargetType.IsInstanceOfType(item);
        }

        public void EntityBeforeSaved(object sender, EntityEventArgs e)
        {
            if (IsTargetEntity(sender))
            {
                OnEntityBeforeSaved(sender, e);
            }
        }

        public void EntityAfterSaved(object sender, EntityEventArgs e)
        {
            if (IsTargetEntity(sender))
            {
                OnEntityAfterSaved(sender, e);
            }
        }
    }
}
