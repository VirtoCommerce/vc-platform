#region
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

#endregion

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Collections : ItemCollection<Collection>
    {
        #region Constructors and Destructors
        public Collections(IEnumerable<Collection> collections)
            : base(collections)
        {
        }
        #endregion

        private Collection _All;
        public Collection All
        {
            get
            {
                if(_All == null)
                {
                    _All = new Collection() { Id = "All" };
                }

                return _All;
            }
        }

        #region Public Methods and Operators
        public override object BeforeMethod(string method)
        {
            return this.Root.SingleOrDefault(x => x.Handle == method);
        }
        #endregion
    }
}