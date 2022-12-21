namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class LoginType
    {
        public bool Enabled { get; set; }

        public bool HasLoginForm { get; set; }

        public string AuthenticationType { get; set; }

        public int Priority { get; set; }
    }
}
