namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class UserLockedResult
    {
        public bool Locked { get; set; }

        public UserLockedResult(bool locked)
        {
            Locked = locked;
        }
    }
}
