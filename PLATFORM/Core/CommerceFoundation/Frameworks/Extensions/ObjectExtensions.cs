using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Frameworks.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToNullOrString(this object o)
        {
            return o == null ? null : o.ToString();
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
    }
}
