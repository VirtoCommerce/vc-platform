using System.Collections.Generic;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class PropertyDictionary : Dictionary<string, object>
    {
        #region Public Methods and Operators

        public void Add(KeyValuePair<string, object> pair)
        {
            this.Add(pair.Key, pair.Value);
        }

        #endregion
    }
}
