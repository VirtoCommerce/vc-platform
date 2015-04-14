namespace VirtoCommerce.Framework.Web.Security
{
    public class RoleSearchRequest
    {
        public string Keyword { get; set; }
        public int SkipCount { get; set; }
        public int TakeCount { get; set; }
    }
}
