namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Association
    {
        #region Public Properties
        /// <summary>
        /// Association description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Association item id (i.e.: product id)
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// Association name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Association priority for sorting
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Association type
        /// </summary>
        public string Type { get; set; }

        #endregion
    }
}
