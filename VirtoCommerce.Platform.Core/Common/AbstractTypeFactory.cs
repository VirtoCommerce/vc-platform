using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Common
{
    /// <summary>
    /// Abstract static type factory. With supports of type overriding and sets special factories.
    /// </summary>
    /// <typeparam name="BaseType"></typeparam>
    public static class AbstractTypeFactory<BaseType>
    {
        private static List<TypeInfo<BaseType>> _typeInfos = new List<TypeInfo<BaseType>>();

        /// <summary>
        /// All registered type mapping informations within current factory instance
        /// </summary>
        public static IEnumerable<TypeInfo<BaseType>> AllTypeInfos
        {
            get
            {
                return _typeInfos;
            }
        }

        /// <summary>
        /// Register new  type (fluent method)
        /// </summary>
        /// <returns>TypeInfo instance to continue configuration through fluent syntax</returns>
        public static TypeInfo<BaseType> RegisterType<T>() where T : BaseType
        {
            var kowTypes = _typeInfos.SelectMany(x => x.AllSubclasses);
            if (kowTypes.Contains(typeof(T)))
            {
                throw new ArgumentException(string.Format("Type {0} already registered", typeof(T).Name));
            }
            var retVal = new TypeInfo<BaseType>(typeof(T));
            _typeInfos.Add(retVal);
            return retVal;
        }

        /// <summary>
        /// Override already registered  type to new 
        /// </summary>
        /// <returns>TypeInfo instance to continue configuration through fluent syntax</returns>
        public static TypeInfo<BaseType> OverrideType<OldType, NewType>() where NewType : BaseType
        {
            var oldType = typeof(OldType);
            var newType = typeof(NewType);
            var existTypeInfo = _typeInfos.FirstOrDefault(x => x.Type == oldType);
            var newTypeInfo = new TypeInfo<BaseType>(newType);
            if (existTypeInfo != null)
            {             
                _typeInfos.Remove(existTypeInfo);
            }

            _typeInfos.Add(newTypeInfo);
            return newTypeInfo;
        }

        /// <summary>
        /// Create BaseType instance considering type mapping information
        /// </summary>
        /// <returns></returns>
        public static BaseType TryCreateInstance()
        {
            return TryCreateInstance(typeof(BaseType).Name);
        }

        /// <summary>
        /// Create derived from BaseType  specified type instance considering type mapping information
        /// </summary>
        /// <returns></returns>
        public static T TryCreateInstance<T>() where T : BaseType
        {
            return (T)TryCreateInstance(typeof(T).Name);
        }

        public static BaseType TryCreateInstance(string typeName)
        {
            BaseType retVal;
            //Try find first direct type match from registered types
            var typeInfo = _typeInfos.FirstOrDefault(x => x.Type.Name.EqualsInvariant(typeName));
            //Then need to find in inheritance chain from registered types
            if (typeInfo == null)
            {                
                typeInfo = _typeInfos.Where(x => x.IsAssignableTo(typeName)).FirstOrDefault();
            }          
            if (typeInfo != null)
            {
                if (typeInfo.Factory != null)
                {
                    retVal = typeInfo.Factory();
                }
                else
                {
                    retVal = (BaseType)Activator.CreateInstance(typeInfo.Type);
                }
            }
            else
            {
                retVal = (BaseType)Activator.CreateInstance(typeof(BaseType));
            }
            return retVal;
        }      
    }

    /// <summary>
    /// Helper class contains  type mapping information
    /// </summary>
    public class TypeInfo<BaseType>
    {
        public TypeInfo(Type type)
        {
            Services = new List<object>();
            Type = type;
        }

        public Func<BaseType> Factory { get; private set; }
        public Type Type { get; private set; }
        public Type MappedType { get; set; }
        public ICollection<object> Services { get; set; }

        public T GetService<T>()
        {
            return Services.OfType<T>().FirstOrDefault(); ;
        }

        public TypeInfo<BaseType> WithService<T>(T service)
        {
            if (!Services.Contains(service))
            {
                Services.Add(service);
            }
            return this;
        }

        public TypeInfo<BaseType> MapToType<T>()
        {
            MappedType = typeof(T);
            return this;
        }

        public TypeInfo<BaseType> WithFactory(Func<BaseType> factory)
        {
            Factory = factory;
            return this;
        }

        public bool IsAssignableTo(string typeName)
        {
            return Type.GetTypeInheritanceChainTo(typeof(BaseType)).Concat(new[] { typeof(BaseType) }).Any(t => typeName.EqualsInvariant(t.Name));
        }

        public IEnumerable<Type> AllSubclasses
        {
            get
            {
                return Type.GetTypeInheritanceChainTo(typeof(BaseType)).ToArray();
            }
        }
    }
}
