namespace VirtoCommerce.Platform.Core.Security
{
    public class RoleSearchRequest
    {
        public RoleSearchRequest()
        {
            Count = 10;
        }

        public string Keyword { get; set; }
        public int Start { get; set; }
        public int Count { get; set; }
    }
}
