using Hangfire.Client;
using Hangfire.Server;
using Microsoft.AspNetCore.Http;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Hangfire.Middleware
{
    /// <summary>
    /// This class allow to process all hangifre jobs to add user name from identity and save it to
    /// the Thread after job is perfoming to achieve getting access to user name in background tasks
    /// </summary>
    public class HangfireUserContextMiddleware : IClientFilter, IServerFilter
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IHangfireDataTransferService _hangfireDataTransferService;

        private const string USER_NAME = "UserName";

        public HangfireUserContextMiddleware(IHttpContextAccessor contextAccessor, IHangfireDataTransferService hangfireDataTransferService)
        {
            _contextAccessor = contextAccessor;
            _hangfireDataTransferService = hangfireDataTransferService;
        }

        private string ContextUserName => _contextAccessor?.HttpContext?.User?.Identity?.Name;

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
            if (userName != null)
            {
                _hangfireDataTransferService.UserName = userName;
            }
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            _hangfireDataTransferService.UserName = null;
        }

        #endregion IServerFilter Members
    }
}
