using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class AbstractTypeFactory<BaseType>
    {
        private static List<TypeInfo<BaseType>> _typeInfos = new List<TypeInfo<BaseType>>();

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
        /// <returns>TypeMappingInfo instance to continue configuration through fluent syntax</returns>
        public static TypeInfo<BaseType> RegisterType<T>() where T : BaseType
        {
            var kowTypes = _typeInfos.SelectMany(x => x.AllInheritedTypes);
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
        /// <returns>TypeMappingInfo instance to continue configuration through fluent syntax</returns>
        public static TypeInfo<BaseType> OverrideType<OldType, NewType>() where NewType : BaseType
        {
            var oldType = typeof(OldType);
            var newType = typeof(NewType);
            var existTypeInfo = _typeInfos.FirstOrDefault(x => x.Type == oldType);
            if (existTypeInfo == null)
            {
                throw new ArgumentException("Not found");
            }
            _typeInfos.Remove(existTypeInfo);

            var newTypeInfo = new TypeInfo<BaseType>(newType)
            {
                Services = existTypeInfo.Services
            };
            _typeInfos.Add(newTypeInfo);
            return newTypeInfo;
        }


        public static BaseType TryCreateInstance(string typeName)
        {
            BaseType retVal = default(BaseType);
            var result = _typeInfos.Select(x => new { TypeInfo = x, Type = x.ResolveTypeByName(typeName) }).FirstOrDefault(x => x.Type != null);
            if (result != null)
            {
                if (result.TypeInfo.Factory != null)
                {
                    retVal = result.TypeInfo.Factory();
                }
                else
                {
                    retVal = (BaseType)Activator.CreateInstance(result.Type);
                }
            }
            return retVal;
        }

    }

    /// <summary>
    /// Helper class contains  type mapping information
    /// </summary>
    public class TypeInfo<BaseType>
    {
        private Func<BaseType> _factory;
        public TypeInfo(Type type)
        {
            Services = new List<object>();
            Type = type;
        }

        public Func<BaseType> Factory { get; set; }
        public Type Type { get; private set; }
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
        public TypeInfo<BaseType> WithFactory(Func<BaseType> factory)
        {
            _factory = factory;
            return this;
        }

        public Type ResolveTypeByName(string typeName)
        {
            if (Type.GetTypeInheritanceChainTo(typeof(BaseType)).Any(t => string.Equals(typeName, t.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return Type;
            }
            return null;
        }

        public IEnumerable<string> AllInheritedTypeNames
        {
            get
            {
                return AllInheritedTypes.Select(x => x.Name);
            }
        }

        public IEnumerable<Type> AllInheritedTypes
        {
            get
            {
                return Type.GetTypeInheritanceChainTo(typeof(BaseType)).ToArray();
            }
        }
    }
}
