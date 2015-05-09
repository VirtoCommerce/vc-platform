#region
using System.Collections.Generic;
using System.Linq;

#endregion

namespace VirtoCommerce.Web.Models
{
    public class MetafieldsCollection : Dictionary<string, object>
    {
        #region Constructors and Destructors
        public MetafieldsCollection(string scope, IDictionary<string, object> collection)
            : base(collection)
        {
            this.Namespace = scope;
        }
        #endregion

        #region Public Properties
        public string Namespace { get; set; }
        #endregion
    }

    public class MetaFieldNamespacesCollection : ItemCollection<MetafieldsCollection>
    {
        #region Constructors and Destructors
        public MetaFieldNamespacesCollection(IEnumerable<MetafieldsCollection> collections)
            : base(collections)
        {
        }

        public MetaFieldNamespacesCollection(MetafieldsCollection collection)
            : base(new[] { collection })
        {
        }
        #endregion

        #region Public Methods and Operators
        public override object BeforeMethod(string method)
        {
            var result = this.Root.SingleOrDefault(x => x.Namespace == method);
            return result;
        }
        #endregion

        #region Public Indexers
        public MetafieldsCollection this[string name]
        {
            get
            {
                var result = this.Root.SingleOrDefault(x => x.Namespace == name);
                return result;
            }
        }

        #endregion

    }
}