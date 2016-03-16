using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VirtoCommerce.Domain.Search.Filters
{
    public class FilterDisplayName
    {
        [XmlAttribute("language")]
        public string Language { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
