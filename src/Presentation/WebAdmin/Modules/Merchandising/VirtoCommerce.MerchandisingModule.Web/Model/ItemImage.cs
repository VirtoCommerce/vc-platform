using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class ItemImage
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Src { get; set; }

        public string ThumbSrc { get; set; }

        public byte[] Attachement { get; set; }
    }
}
