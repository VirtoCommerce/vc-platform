namespace VirtoCommerce.ApiClient.DataContracts.Lists
{
    public class Link
    {
        #region Public Properties
        public string Url { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public bool IsActive { get; set; }

        public int Priority { get; set; }
        #endregion
    }
}
