namespace VirtoCommerce.Platform.Core.Security
{
    public class SecurityResult
    {
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
    }

    public class UserLockedResult
    {
        public bool Locked { get; set; }

        public UserLockedResult(bool locked)
        {
            Locked = locked;
        }
    }
}
