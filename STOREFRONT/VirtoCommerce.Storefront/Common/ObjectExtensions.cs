using Newtonsoft.Json.Linq;

namespace VirtoCommerce.Storefront.Common
{
    public static class ObjectExtensions
    {
        public static T JsonClone<T>(this T source)
        {
            var jObject = JObject.FromObject(source);
            var result = jObject.ToObject<T>();
            return result;
        }
    }
}
