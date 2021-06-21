using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Common
{
    public class Country
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IList<CountryRegion> Regions { get; set; }
    }
}
