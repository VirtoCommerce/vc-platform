namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Image
    {
        #region Public Properties

        public byte[] Attachement { get; set; }

        public string Name { get; set; }
		public string Group { get; set; }
        public string Src { get; set; }

        public string ThumbSrc { get; set; }

        #endregion
    }
}
