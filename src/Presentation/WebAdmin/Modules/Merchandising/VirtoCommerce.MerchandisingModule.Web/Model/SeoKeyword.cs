using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class SeoKeyword
    {
        public string Id { get; set; }
        public string Keyword { get; set; }
        public string KeywordValue { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string ImageAltDescription { get; set; }
        public string Language { get; set; }
        public int KeywordType { get; set; }
        public string MetaKeywords { get; set; }

    }

}
