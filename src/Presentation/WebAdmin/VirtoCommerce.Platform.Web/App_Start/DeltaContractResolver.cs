using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using System;
using System.Dynamic;
using System.Net.Http.Formatting;
using System.Web.Http.OData;

namespace VirtoCommerce.Platform.Web
{
    using System.Collections;
    using System.Linq;
    using System.Reflection;

    using Newtonsoft.Json;

    /// <summary>
    /// This class implements a JsonContractResolver to provide support for deserialization of the Delta<T> type
    /// using Json.NET. 
    /// </summary>
    /// <remarks>
    /// See WebApiConfig.cs to see how the this class is used in the configuration.
    ///
    /// The contract created for Delta<T> will deserialize properties using the types and property names of the 
    /// underlying type. The JsonProperty instances are copied from the underlying type's JsonContract and 
    /// customized to work with a dynamic object. In particular, a custom IValueProvider is used to get and set 
    /// values using the contract of DynamicObject, which Delta<T> inherits from.
    /// </remarks>
    public class DeltaContractResolver : CamelCasePropertyNamesContractResolver // JsonContractResolver
    {
       private readonly MediaTypeFormatter _formatter;

       public DeltaContractResolver(MediaTypeFormatter formatter)
        {
            _formatter = formatter;
            // Need this setting to have [Serializable] types serialized correctly
            IgnoreSerializableAttribute = false;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            // This class special cases the JsonContract for just the Delta<T> class. All other types should function
            // as usual.
            if (objectType.IsGenericType &&
                objectType.GetGenericTypeDefinition() == typeof(Delta<>) &&
                objectType.GetGenericArguments().Length == 1)
            {
                var contract = CreateDynamicContract(objectType);
                contract.Properties.Clear();

                var underlyingContract = CreateObjectContract(objectType.GetGenericArguments()[0]);
                var underlyingProperties =
                    underlyingContract.CreatedType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var property in underlyingContract.Properties)
                {
                    property.DeclaringType = objectType;
                    property.ValueProvider = new DynamicObjectValueProvider()
                    {
                        PropertyName = this.ResolveName(underlyingProperties, property.PropertyName),
                    };

                    contract.Properties.Add(property);
                }

                return contract;
            }

            return base.CreateContract(objectType);
        }

        /// <summary>
        /// Resolves property ignoring casing
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        internal string ResolveName(IEnumerable<PropertyInfo> properties, string propertyName)
        {
            if (properties == null) throw new ArgumentNullException("properties");

            var prop = properties.SingleOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

            return prop != null ? prop.Name : propertyName;
        }

        // Determines whether a member is required or not and sets the appropriate JsonProperty settings
        private void ConfigureProperty(MemberInfo member, JsonProperty property)
        {
            if (_formatter.RequiredMemberSelector != null && _formatter.RequiredMemberSelector.IsRequiredMember(member))
            {
                property.Required = Required.AllowNull;
                property.DefaultValueHandling = DefaultValueHandling.Include;
                property.NullValueHandling = NullValueHandling.Include;
            }
            else
            {
                property.Required = Required.Default;
            }
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            ConfigureProperty(member, property);
            return property;
        }

        private class DynamicObjectValueProvider : IValueProvider
        {
            public string PropertyName
            {
                get;
                set;
            }

            public object GetValue(object target)
            {
                DynamicObject d = (DynamicObject)target;

                object result;
                var binder = CreateGetMemberBinder(target.GetType(), PropertyName);
                d.TryGetMember(binder, out result);
                return result;
            }

            public void SetValue(object target, object value)
            {
                DynamicObject d = (DynamicObject)target;
                var binder = CreateSetMemberBinder(target.GetType(), PropertyName);
                d.TrySetMember(binder, value);
            }

            /**
            public void SetValue(object target, object value)
            {
                DynamicObject d = (DynamicObject)target;

                Type propertyType = GetPropertyType(d, PropertyName);

                Type elementType;
                if (!IsCollection(propertyType, out elementType))
                {
                }

                IEnumerable newCollection;
                if (TryCreateInstance(propertyType, elementType, out newCollection))
                {
                    // settable collections
                    MethodInfo addMethod = null;
                    addMethod = newCollection.GetType().GetMethod("Add", new Type[] { elementType });
                }
                var binder = CreateSetMemberBinder(target.GetType(), PropertyName);
                d.TrySetMember(binder, value);
            }* 
             * */
        }

        private static GetMemberBinder CreateGetMemberBinder(Type type, string memberName)
        {
            return (GetMemberBinder)Microsoft.CSharp.RuntimeBinder.Binder.GetMember(
                Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags.None,
                memberName,
                type,
                new Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo[] 
                {
                });
        }


        private static SetMemberBinder CreateSetMemberBinder(Type type, string memberName)
        {
            return (SetMemberBinder)Microsoft.CSharp.RuntimeBinder.Binder.SetMember(
                Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags.None,
                memberName,
                type,
                new Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo[] 
                {
                    Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags.None, null),
                });
        }

        public static bool TryCreateInstance(Type collectionType, Type elementType, out IEnumerable instance)
        {

            if (collectionType.IsGenericType)
            {
                Type genericDefinition = collectionType.GetGenericTypeDefinition();
                if (genericDefinition == typeof(IEnumerable<>) ||
                    genericDefinition == typeof(ICollection<>) ||
                    genericDefinition == typeof(IList<>))
                {
                    instance = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType)) as IEnumerable;
                    return true;
                }
            }

            if (collectionType.IsArray)
            {
                // We dont know the size of the collection in advance. So, create a list and later call ToArray. 
                instance = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType)) as IEnumerable;
                return true;
            }

            if (collectionType.GetConstructor(Type.EmptyTypes) != null && !collectionType.IsAbstract)
            {
                instance = Activator.CreateInstance(collectionType) as IEnumerable;
                return true;
            }

            instance = null;
            return false;
        }

        internal static Type GetPropertyType(object resource, string propertyName)
        {
            IDelta delta = resource as IDelta;
            if (delta != null)
            {
                Type type;
                delta.TryGetPropertyType(propertyName, out type);
                return type;
            }
            else
            {
                PropertyInfo property = resource.GetType().GetProperty(propertyName);
                return property == null ? null : property.PropertyType;
            }
        }

        public static bool IsCollection(Type type, out Type elementType)
        {
            elementType = type;

            // see if this type should be ignored.
            if (type == typeof(string))
            {
                return false;
            }

            Type collectionInterface
                = type.GetInterfaces()
                    .Union(new[] { type })
                    .FirstOrDefault(
                        t => t.IsGenericType
                             && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (collectionInterface != null)
            {
                elementType = collectionInterface.GetGenericArguments().Single();
                return true;
            }

            return false;
        }
    }
}
