using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class Collections : ItemCollection<Collection>
    {
        public Collections(IEnumerable<Collection> collections)
            : base(collections)
        {
        }

        #region Public Methods and Operators
        public override object BeforeMethod(string method)
        {
            return this.SingleOrDefault(x => x.Handle == method);
        }
        #endregion
        
    }
}
