namespace VirtoCommerce.ApiClient.DataContracts.Security
{
    public class UserLoginInfo
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }

        public string UserId { get; set; }
    }
}