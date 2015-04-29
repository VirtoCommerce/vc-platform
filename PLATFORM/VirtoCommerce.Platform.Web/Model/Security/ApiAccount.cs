namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class ApiAccount
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public string AppId { get; set; }
        public string SecretKey { get; set; }
    }
}
