using System;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Data.Security
{
    public class UserNameResolver : IUserNameResolver
    {
        private readonly Func<ICurrentUser> _currentUserFactory;

        public UserNameResolver(Func<ICurrentUser> currentUserFactory)
        {
            _currentUserFactory = currentUserFactory;
        }

        public string GetCurrentUserName()
        {
            var currentUser = _currentUserFactory != null ? _currentUserFactory() : null;
            var userName = currentUser != null ? currentUser.UserName : null;

            if (string.IsNullOrEmpty(userName))
            {
                userName = "unknown";
            }

            return userName;
        }
    }
}
