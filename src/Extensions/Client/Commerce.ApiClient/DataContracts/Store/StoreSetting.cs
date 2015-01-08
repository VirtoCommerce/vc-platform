using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient.DataContracts.Store
{
    public class StoreSetting
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public string Locale { get; set; }
    }
}
