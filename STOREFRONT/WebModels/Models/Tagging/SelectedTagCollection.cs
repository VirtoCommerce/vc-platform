using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using VirtoCommerce.Web.Filters;
using VirtoCommerce.Web.Models.Filters;

namespace VirtoCommerce.Web.Models.Tagging
{
    [DataContract]
    public class SelectedTagCollection : ItemCollection<string>, ILiquidContains
    {
        #region Constructors and Destructors
        public SelectedTagCollection(IEnumerable<string> tags)
            : base(tags)
        {
        }

        #endregion

        #region Implementation of ILiquidContains
        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.IList"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Object"/> is found in the <see cref="T:System.Collections.IList"/>; otherwise, false.
        /// </returns>
        /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList"/>. </param>
        public bool Contains(object value)
        {
            if (this.Root != null)
            {
                if (value is Tag)
                {
                    var tag = value as Tag;
                    return Root.Any(x => x.Equals(tag.Id));
                }

                return Root.Any(x => x.Equals(
                    value.ToString()) || x.Equals(ModelFilters.Handleize(value.ToString())));
            }

            return false;
        }
        #endregion
    }
}
