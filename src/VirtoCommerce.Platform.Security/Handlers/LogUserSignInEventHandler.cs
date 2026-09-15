using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Security.Handlers
{
    /// <summary>
    /// Turns a <see cref="UserSignInAttemptEvent"/> into a <see cref="UserSignInLog"/> row.
    /// Enriches it with request context and whatever modules contribute, then hands it to the
    /// buffered writer. Never throws — a failing audit log must not fail a sign-in.
    /// </summary>
    public class LogUserSignInEventHandler : IEventHandler<UserSignInAttemptEvent>
    {
        private readonly IUserSignInLogWriter _writer;
        private readonly ISettingsManager _settingsManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEnumerable<IUserSignInLogEnricher> _enrichers;

        public LogUserSignInEventHandler(
            IUserSignInLogWriter writer,
            ISettingsManager settingsManager,
            IHttpContextAccessor httpContextAccessor,
            IEnumerable<IUserSignInLogEnricher> enrichers)
        {
            _writer = writer;
            _settingsManager = settingsManager;
            _httpContextAccessor = httpContextAccessor;
            _enrichers = enrichers;
        }

        public virtual async Task Handle(UserSignInAttemptEvent message)
        {
            if (!await ShouldLogAsync(message))
            {
                return;
            }

            var record = AbstractTypeFactory<UserSignInLog>.TryCreateInstance();

            record.CreatedDate = DateTime.UtcNow;
            record.UserName = message.UserName;
            record.UserId = message.UserId;
            record.Succeeded = message.Succeeded;
            record.FailureReason = message.FailureReason;
            record.SignInType = message.SignInType;
            record.Provider = message.Provider;
            record.OperatorUserId = message.OperatorUserId;
            record.OperatorUserName = message.OperatorUserName;
            record.ClientId = message.ClientId;
            record.SessionId = message.SessionId;
            record.StoreId = message.StoreId;
            record.MemberId = message.MemberId;

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                record.IpAddress = httpContext.Connection.RemoteIpAddress?.ToString();
                record.UserAgent = httpContext.Request.Headers.UserAgent.ToString().EmptyToNull();
            }

            await EnrichAsync(record);

            _writer.Write(record);
        }

        /// <summary>
        /// Impersonation is always logged, whatever the setting says: it is roughly ten rows a day and
        /// it is the compliance anchor. A switch that silently disables it will eventually be flipped.
        /// </summary>
        protected virtual async Task<bool> ShouldLogAsync(UserSignInAttemptEvent message)
        {
            if (message.SignInType == SignInType.Impersonation ||
                message.SignInType == SignInType.ImpersonationRevert)
            {
                return true;
            }

            return await _settingsManager.GetValueAsync<bool>(PlatformConstants.Settings.Security.SignInLogEnabled);
        }

        /// <summary>
        /// A broken enricher must never block a sign-in or lose a row — the record is written with
        /// whatever enrichment succeeded.
        /// </summary>
        protected virtual async Task EnrichAsync(UserSignInLog record)
        {
            foreach (var enricher in _enrichers.OrderBy(x => x.Priority))
            {
                try
                {
                    await enricher.EnrichAsync(record);
                }
                catch
                {
                    // Intentionally swallowed: the record still lands, just with less context.
                }
            }
        }
    }
}
