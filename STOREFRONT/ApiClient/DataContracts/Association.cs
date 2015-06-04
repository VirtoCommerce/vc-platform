namespace VirtoCommerce.ApiClient.DataContracts
{
    public class Association
    {
        #region Public Properties

        public string Description { get; set; }

        public string ItemId { get; set; }

        public string Name { get; set; }

        public int Priority { get; set; }

        public string Type { get; set; }

        #endregion
    }

    public enum AssociationTypes
    {
        required,

        optional
    }
}
