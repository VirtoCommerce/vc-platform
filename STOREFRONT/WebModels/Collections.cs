#region
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

#endregion

namespace VirtoCommerce.Web.Models
{

    #region
    #endregion

    [DataContract]
    public class Collections : ItemCollection<Collection>
    {
        #region Constructors and Destructors
        public Collections(IEnumerable<Collection> collections)
            : base(collections)
        {
        }
        #endregion

        #region Public Methods and Operators
        public override object BeforeMethod(string method)
        {
            return this.Root.SingleOrDefault(x => x.Handle == method);
        }
        #endregion
    }
}