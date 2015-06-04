namespace VirtoCommerce.Platform.Core.Security
{
    public class RoleSearchRequest
    {
        public RoleSearchRequest()
        {
            TakeCount = 10;
        }

        public string Keyword { get; set; }
        public int SkipCount { get; set; }
        public int TakeCount { get; set; }
    }
}
