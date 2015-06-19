using DotLiquid;

namespace VirtoCommerce.Web.Models.Storage
{
    public class DownloadLink : Drop
    {
        public string Text { get; set; }

        public string Filename { get; set; }

        public string Url { get; set; }
    }
}