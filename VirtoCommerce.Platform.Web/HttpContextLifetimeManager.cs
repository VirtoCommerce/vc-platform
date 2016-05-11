using System.Web;
using Microsoft.Practices.Unity;

namespace VirtoCommerce.Platform.Web
{
    public class HttpContextLifetimeManager : LifetimeManager
    {
        private readonly object _key = new object();

        #region Overrides of LifetimeManager

        /// <summary>
        /// Retrieve a value from the backing store associated with this Lifetime policy.
        /// </summary>
        /// <returns>
        /// the object desired, or null if no such object is currently stored.
        /// </returns>
        public override object GetValue()
        {
            object result;

            if (HttpContext.Current != null && HttpContext.Current.Items.Contains(_key))
            {
                result = HttpContext.Current.Items[_key];
            }
            else
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Stores the given value into backing store for retrieval later.
        /// </summary>
        /// <param name="newValue">The object being stored.</param>
        public override void SetValue(object newValue)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items[_key] = newValue;
            }
        }

        /// <summary>
        /// Remove the given object from backing store.
        /// </summary>
        public override void RemoveValue()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items.Remove(_key);
            }
        }

        #endregion
    }
}
