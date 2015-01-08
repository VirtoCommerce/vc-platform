using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient.DataContracts
{
    public class Category
    {
        public string Id { get; set; }

        public string ParentId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> Parents { get; set; }

        public bool Virtual { get; set; }

        public string Outline
        {
            get
            {
                var ids = Parents != null ? Parents.Select(x => x.Key).ToList() : new List<string>();
                ids.Add(Id);
                return string.Join("/", ids);
            }
        }
    }
}
