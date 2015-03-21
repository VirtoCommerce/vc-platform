using System.Collections.Generic;
using System.Dynamic;

namespace VirtoCommerce.Web.Client.Extensions
{
	/// <summary>
	/// Class ExpandoExtensions.
	/// </summary>
    public static class ExpandoExtensions
    {
		/// <summary>
		/// To the expando.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns>ExpandoObject.</returns>
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