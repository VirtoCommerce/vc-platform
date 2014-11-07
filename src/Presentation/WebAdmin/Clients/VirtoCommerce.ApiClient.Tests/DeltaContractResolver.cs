using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.OData;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VirtoCommerce.ApiClient.Tests
{
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
    public class DeltaContractResolver : JsonContractResolver
    {
        public DeltaContractResolver(MediaTypeFormatter formatter)
            : base(formatter)
        {
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
                foreach (var property in underlyingContract.Properties)
                {
                    property.DeclaringType = objectType;
                    property.ValueProvider = new DynamicObjectValueProvider()
                    {
                        PropertyName = property.PropertyName,
                    };

                    contract.Properties.Add(property);
                }

                return contract;
            }

            return base.CreateContract(objectType);
        }

        /*
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == typeof (Employee))
            {
                property.ShouldSerialize =
            }

            return property;
        }
         * */

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
    }
}
