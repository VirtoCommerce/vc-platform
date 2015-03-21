using Newtonsoft.Json;

namespace VirtoCommerce.ApiClient.DataContracts
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
