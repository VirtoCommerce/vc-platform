namespace VirtoCommerce.ApiClient.DataContracts.CustomerService
{
    public class ContactProperty
    {
        #region Public Properties

        public string Name { get; set; }
		public ContactPropertyValue[] Values { get; set; }
        public string ValueType { get; set; }

        #endregion
    }
}
