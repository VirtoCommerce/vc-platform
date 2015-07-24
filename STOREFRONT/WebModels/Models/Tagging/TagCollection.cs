using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Models.Tagging
{
    [DataContract]
    public class TagCollection : ItemCollection<Tag>
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
    }
}
