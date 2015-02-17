#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace VirtoCommerce.Web.Core.DataContracts
{
    #region
    
    #endregion

    public class TagQuery : Dictionary<string, object>
    {
        #region Public Properties
        /// <summary>
        ///     Gets the names.
        /// </summary>
        /// <value>
        ///     The names.
        /// </value>
        public string[] Names
        {
            get
            {
                return this.Keys.ToArray();
            }
        }
        #endregion

        #region Public Methods and Operators
        /// <summary>
        ///     Adds the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="tag">The tag.</param>
        public new void Add(string name, object tag)
        {
            if (this.ContainsKey(name))
            {
                this.Remove(name);
            }
            base.Add(name, tag);
        }

        /// <summary>
        ///     Gets the cache key.
        /// </summary>
        /// <returns></returns>
        public string GetCacheKey()
        {
            var builder = new StringBuilder();
            foreach (var name in this.Names)
            {
                var value = this[name];
                builder.Append(String.Format("{0}-{1};", name, value != null ? value.ToString() : String.Empty));
            }

            return builder.ToString();
        }
        #endregion
    }
}