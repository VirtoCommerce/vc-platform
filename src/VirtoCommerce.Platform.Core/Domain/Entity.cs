using System;

namespace VirtoCommerce.Platform.Core.Common
{
    public abstract class Entity : IEntity
    {
        public string Id { get; set; }

        public bool IsTransient()
        {
            return Id == null;
        }

        #region Overrides Methods

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (ReferenceEquals(null, obj))
                return false;

            if (GetRealObjectType(this) != GetRealObjectType(obj))
                return false;


            var other = obj as Entity;
            return other != null && Id == other.Id;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return IsTransient() ? base.GetHashCode() : Id.GetHashCode();
            }
        }
        public static bool operator ==(Entity left, Entity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !Equals(left, right);
        }

        #endregion

        private Type GetRealObjectType(object obj)
        {
            var retVal = obj.GetType();
            //because can be compared two object with same id and 'types' but one of it is EF dynamic proxy type)
            if (retVal.BaseType != null && retVal.Namespace == "System.Data.Entity.DynamicProxies")
            {
                retVal = retVal.BaseType;
            }
            return retVal;
        }
    }
}
