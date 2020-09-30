using System.Threading;
using Hangfire.Client;
using Hangfire.Server;
using Microsoft.AspNetCore.Http;
using VirtoCommerce.Platform.Security;

namespace VirtoCommerce.Platform.Web.Middleware.Hangfire
{
    public class HangfireUserContextMiddleware : IClientFilter, IServerFilter
    {
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Ctor
        /// </summary>
        public HangfireUserContextMiddleware(IHttpContextAccessor contextAccessor) => _contextAccessor = contextAccessor;

        private string ContextUserName => _contextAccessor?.HttpContext?.User?.Identity?.Name;

        #region IClientFilter Members

        public void OnCreating(CreatingContext filterContext)
        {
            if (ContextUserName is null)
                return;

            filterContext.SetJobParameter(HttpContextUserResolver.USER_NAME_THREAD_SLOT_NAME, ContextUserName);
        }

        public void OnCreated(CreatedContext filterContext)
        {
            // Pass
        }

        #endregion IClientFilter Members

        #region IServerFilter Members

        public void OnPerforming(PerformingContext filterContext)
        {
            var userName = filterContext.GetJobParameter<string>(HttpContextUserResolver.USER_NAME_THREAD_SLOT_NAME);

            Thread.SetData(Thread.GetNamedDataSlot(HttpContextUserResolver.USER_NAME_THREAD_SLOT_NAME), userName);
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            // Pass
        }

        #endregion IServerFilter Members
    }
}
