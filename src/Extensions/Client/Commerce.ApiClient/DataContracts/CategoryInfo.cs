using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient.DataContracts
{
    public class CategoryInfo
    {
        public string Id { get; set; }
        public SeoKeyword[] SeoKeywords { get; set; }
        public string Name { get; set; }
    }
}
