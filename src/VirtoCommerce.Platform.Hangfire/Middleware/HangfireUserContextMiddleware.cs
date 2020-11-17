using System.Threading;
using Hangfire.Client;
using Hangfire.Server;
using VirtoCommerce.Platform.Core.Security;
using static VirtoCommerce.Platform.Core.Common.ThreadSlotNames;

namespace VirtoCommerce.Platform.Hangfire.Middleware
{
    /// <summary>
    /// This class allow to process all hangifre jobs to add user name from identity and save it to
    /// the Thread after job is perfoming to achieve getting access to user name in background tasks
    /// </summary>
    public class HangfireUserContextMiddleware : IClientFilter, IServerFilter
    {
        private readonly IUserNameResolver _userNameResolver;

        public HangfireUserContextMiddleware(IUserNameResolver userNameResolver)
        {
            _userNameResolver = userNameResolver;
        }

        private string ContextUserName => _userNameResolver.GetCurrentUserName();

        #region IClientFilter Members

        public void OnCreating(CreatingContext filterContext)
        {
            if (ContextUserName is null)
                return;

            filterContext.SetJobParameter(USER_NAME, ContextUserName);
        }

        public void OnCreated(CreatedContext filterContext)
        {
            // Pass
        }

        #endregion IClientFilter Members

        #region IServerFilter Members

        public void OnPerforming(PerformingContext filterContext)
        {
            var userName = filterContext.GetJobParameter<string>(USER_NAME);

            Thread.SetData(Thread.GetNamedDataSlot(USER_NAME), userName);
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            // Pass
        }

        #endregion IServerFilter Members
    }
}
