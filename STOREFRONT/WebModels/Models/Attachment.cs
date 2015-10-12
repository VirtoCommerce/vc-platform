using DotLiquid;

namespace VirtoCommerce.Web.Models
{
    public class Attachment : Drop
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string MimeType { get; set; }

        public long Size { get; set; }
    }
}