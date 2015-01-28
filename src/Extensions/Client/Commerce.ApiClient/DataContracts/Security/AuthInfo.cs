using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Security
{
    public class AuthInfo
    {
        public string Id { get; set; }
        public int AccountState { get; set; }
        public Dictionary<string, string> Properties { get; set; }
        public string Login { get; set; }
        public string FullName { get; set; }
        public string[] Permissions { get; set; }
        public RegisterType UserType { get; set; }
    }

    public enum RegisterType
    {
        GuestUser,
        RegisteredUser,
        Administrator,
        SiteAdministrator
    }

}
