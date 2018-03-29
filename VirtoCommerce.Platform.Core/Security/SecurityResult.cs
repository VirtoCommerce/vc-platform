using System;

namespace VirtoCommerce.Platform.Core.Security
{
    public class SecurityResult
    {
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
    }

    public class UserLockedResult
    {
        public DateTimeOffset LockoutEndDate { get; set; }

        public bool LockoutEnable { get; set; }
        public bool Locked { get; set; }

        public UserLockedResult()
        {
        }

        public UserLockedResult(bool locked)
        {
            Locked = locked;
        }
    }
}
