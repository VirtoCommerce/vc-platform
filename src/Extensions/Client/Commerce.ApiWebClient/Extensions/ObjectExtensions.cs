using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;

namespace VirtoCommerce.ApiWebClient.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToNullOrString(this object o)
        {
            return o == null ? null : o.ToString();
        }

        /// <summary>
        /// Builds a dictionary from the object's properties
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToPropertyDictionary(this object o)
        {
            if (o == null)
                return null;
            return o.GetType().GetProperties()
                .Select(p => new KeyValuePair<string, object>(p.Name, p.GetValue(o, null)))
                .ToDictionary();
        }

        public static T Clone<T>(this T obj) where T : class
        {
            if (Attribute.GetCustomAttribute(typeof(T), typeof(DataContractAttribute)) == null)
                return null;

            var serializer = new DataContractSerializer(typeof(T), null, int.MaxValue, false, true, null);
            using (var ms = new System.IO.MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                ms.Position = 0;
                return (T)serializer.ReadObject(ms);
            }
        }

        public static ExpandoObject ToExpando(this object o)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (var propertyInfo in o.GetType().GetProperties())
            {
                expando.Add(new KeyValuePair<string, object>(propertyInfo.Name, propertyInfo.GetValue(o, index: null)));
            }

            return (ExpandoObject)expando;
        }
    }
}