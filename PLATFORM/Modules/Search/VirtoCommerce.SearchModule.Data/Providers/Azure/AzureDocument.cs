using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.SearchModule.Data.Provides.Azure
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
