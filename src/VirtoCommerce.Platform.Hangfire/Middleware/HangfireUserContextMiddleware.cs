using Hangfire.Client;
using Hangfire.Server;
using VirtoCommerce.Platform.Core.Security;
using static VirtoCommerce.Platform.Core.Common.ThreadSlotNames;

namespace VirtoCommerce.Platform.Hangfire.Middleware
{
    /// <summary>
    /// This class allow to process all HangFire jobs to add user name from identity and save it to
    /// the Thread after job is performing to achieve getting access to user name in background tasks
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
            {
                return;
            }

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
            string userName;

            if (IsRecurringJob(filterContext, out var recurringJobId))
            {
                userName = $"system:{recurringJobId}";
            }
            else
            {
                userName = filterContext.GetJobParameter<string>(USER_NAME);
            }

            _userNameResolver.SetCurrentUserName(userName);
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            // Pass
        }

        #endregion IServerFilter Members

        private static bool IsRecurringJob(PerformingContext context, out string recurringJobId)
        {
            recurringJobId = context.GetJobParameter<string>("RecurringJobId");
            return !string.IsNullOrEmpty(recurringJobId);
        }
    }
}
