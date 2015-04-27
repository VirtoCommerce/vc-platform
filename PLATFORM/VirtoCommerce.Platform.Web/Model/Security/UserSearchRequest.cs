namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class UserSearchRequest
    {
        public UserSearchRequest()
        {
            TakeCount = 20;
        }

        public string Keyword { get; set; }
        public int SkipCount { get; set; }
        public int TakeCount { get; set; }
    }
}
