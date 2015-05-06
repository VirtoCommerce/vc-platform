using System.Collections.Generic;

namespace VirtoCommerce.SearchModule.Data.Providers.Azure
{
    public class AzureDocument : Dictionary<string, object>
    {
        public static string KeyFieldName = "sys__key";
        public object Id
        {
            get
            {
                if (this.ContainsKey(KeyFieldName))
                    return this[KeyFieldName];

                return null;
            }
            set
            {
                if (this.ContainsKey(KeyFieldName))
                    this[KeyFieldName] = value;
                else
                    this.Add(KeyFieldName, value);
            }
        }
    }

}
