using System;
using System.Linq;
using System.Threading;
using Hangfire.Client;
using Hangfire.Server;
using Microsoft.AspNetCore.Http;
using VirtoCommerce.Platform.Core.ChangeLog;
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

        /// <summary>
        /// Check if argument contains information about log operation
        /// </summary>
        private readonly Func<object, bool> AuditableArgument = arg => arg is OperationLog || arg is OperationLog[];

        /// <summary>
        /// Check if job contains operation log
        /// </summary>
        /// <param name="context">Context before job is created</param>
        /// <returns>True or false</returns>
        private bool IsAuditableOperation(CreatingContext context) => context.Job.Args.Any(AuditableArgument);

        /// <summary>
        /// Check if job contains operation log
        /// </summary>
        /// <param name="context">Context when job is started</param>
        /// <returns>True or false</returns>
        private bool IsAuditableOperation(PerformingContext context) => context.BackgroundJob.Job.Args.Any(AuditableArgument);

        private string ContextUserName => _contextAccessor?.HttpContext?.User?.Identity?.Name;

        #region IClientFilter Members

        public void OnCreating(CreatingContext filterContext)
        {
            if (!IsAuditableOperation(filterContext))
                return;

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
            if (!IsAuditableOperation(filterContext))
                return;

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
