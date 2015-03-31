using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient.DataContracts.Pages
{
    public class Page
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Language { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
