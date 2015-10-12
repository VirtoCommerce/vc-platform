using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using VirtoCommerce.Web.Filters;

namespace VirtoCommerce.Web.Models.Tagging
{
    [DataContract]
    public class TagCollection : ItemCollection<Tag>, ILiquidContains
    {
        #region Constructors and Destructors
        public TagCollection(IEnumerable<Tag> tags)
            : base(tags)
        {
        }

        #endregion

        public IEnumerable<string> Groups {
            get
            {
                if (this.Root != null)
                {
                    return this.Root.GroupBy(x => x.Field).Select(grp => grp.First().Field);
                }

                return null;
            }
        }

        #region Implementation of ILiquidContains

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.IList"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Object"/> is found in the <see cref="T:System.Collections.IList"/>; otherwise, false.
        /// </returns>
        /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList"/>. </param>
        public virtual bool Contains(object value)
        {
            if (this.Root == null)
                return false;

            foreach (var item in this.Root)
            {
                if (item.Equals(value))
                    return true;
            }

            return false;
        }
        #endregion
    }
}
