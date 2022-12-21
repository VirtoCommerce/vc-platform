namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class VerifyTokenRequest
    {
        public string TokenProvider { get; set; }
        public string Purpose { get; set; }
        public string Token { get; set; }
    }
}
