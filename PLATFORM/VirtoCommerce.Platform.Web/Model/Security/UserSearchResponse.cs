namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class UserSearchResponse
    {
        public ApplicationUserExtended[] Users { get; set; }
        public int TotalCount { get; set; }
    }
}
