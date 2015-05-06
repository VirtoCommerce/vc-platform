using DotLiquid;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Web.Models
{
    public class CountryOptionTags : Drop
    {
        private const string tagTemplate = "<option value=\"{0}\" data-provinces=\"{1}\">{0}</option>\r\n";
        private IDictionary<string, ICollection<string>> _countries;

        public CountryOptionTags(Dictionary<string, ICollection<string>> countries)
        {
            _countries = countries;
        }

        public override string ToString()
        {
            var tags = new StringBuilder();

            foreach (var country in _countries)
            {
                var provinces = new StringBuilder("[");

                int i = 1;
                foreach (var province in country.Value.OrderBy(p => p))
                {
                    provinces.AppendFormat("&quot;{0}&quot;", province);

                    if (i < country.Value.Count)
                    {
                        provinces.Append(",");
                    }

                    i++;
                }

                provinces.Append("]");

                tags.AppendFormat(tagTemplate, country.Key, provinces.ToString());
            }

            return tags.ToString();
        }
    }
}