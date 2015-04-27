namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class UserSearchCriteria
    {
        public UserSearchCriteria()
        {
            Count = 20;
        }

        public string Keyword { get; set; }

        public int Start { get; set; }

        public int Count { get; set; }
    }
}