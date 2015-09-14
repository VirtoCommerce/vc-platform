using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts
{
    public class DynamicObjectProperty
    {
        public string ObjectId { get; set; }

        public ICollection<DynamicPropertyObjectValue> Values { get; set; }
    }
}