using System;

namespace VirtoCommerce.ApiClient.DataContracts.Themes
{
    public class ThemeAsset
    {
        #region Public Properties

        public string Content { get; set; }

        public byte[] ByteContent { get; set; }

        public string ContentType { get; set; }

        public string Id { get; set; }

        public DateTime Updated { get; set; }

        #endregion
    }
}
