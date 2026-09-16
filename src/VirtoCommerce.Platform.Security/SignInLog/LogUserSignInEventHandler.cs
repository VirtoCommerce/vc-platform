using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Core.Security.SignInLog;

namespace VirtoCommerce.Platform.Security.SignInLog
{
    /// <summary>
    /// Turns a <see cref="UserSignInAttemptEvent"/> into a <see cref="UserSignInLog"/> row, stamps it
    /// with the request context only this thread can see, and hands it to the buffered writer.
    /// Module enrichment happens later, on the writer's flush loop, so nothing a module does lands on
    /// the sign-in latency path. Never throws — a failing audit log must not fail a sign-in.
    /// </summary>
    public class LogUserSignInEventHandler : IEventHandler<UserSignInAttemptEvent>
    {
        // Column lengths from SecurityDbContext. User agent and user name are unvalidated client
        // input, and an over-length value would fail the whole batched insert, discarding every
        // other record in it - including impersonation rows.
        private const int UserNameLength = 256;
        private const int UserAgentLength = 512;

        private readonly IUserSignInLogWriter _writer;
        private readonly ISettingsManager _settingsManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<LogUserSignInEventHandler> _logger;

        public LogUserSignInEventHandler(
            IUserSignInLogWriter writer,
            ISettingsManager settingsManager,
            IHttpContextAccessor httpContextAccessor,
            ILogger<LogUserSignInEventHandler> logger)
        {
            _writer = writer;
            _settingsManager = settingsManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// Never throws. This runs inside the sign-in request, so an audit failure must not become
        /// an authentication failure - the whole point of the buffered writer is that recording an
        /// attempt cannot break the attempt.
        /// </summary>
        public virtual async Task Handle(UserSignInAttemptEvent message)
        {
            try
            {
                await HandleCore(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to record sign-in attempt for {UserName}.", message?.UserName);
            }
        }

        protected virtual async Task HandleCore(UserSignInAttemptEvent message)
        {
            if (!await ShouldLog(message))
            {
                return;
            }

            var record = AbstractTypeFactory<UserSignInLog>.TryCreateInstance();

            record.CreatedDate = DateTime.UtcNow;
            record.UserName = Truncate(message.UserName, UserNameLength);
            record.UserId = message.UserId;
            record.Succeeded = message.Succeeded;
            record.FailureReason = message.FailureReason;
            record.SignInType = message.SignInType;
            record.Provider = message.Provider;
            record.OperatorUserId = message.OperatorUserId;
            record.OperatorUserName = Truncate(message.OperatorUserName, UserNameLength);
            record.ClientId = message.ClientId;
            record.SessionId = message.SessionId;
            record.StoreId = message.StoreId;
            record.MemberId = message.MemberId;

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                record.IpAddress = httpContext.Connection.RemoteIpAddress?.ToString();
                record.UserAgent = Truncate(httpContext.Request.Headers.UserAgent.ToString().EmptyToNull(), UserAgentLength);
            }

            // Request context is captured here because only this thread has it; everything a module
            // has to look up is enriched later, on the writer's flush loop, off the sign-in path.
            _writer.Write(record);
        }

        private static string Truncate(string value, int maxLength)
        {
            return value != null && value.Length > maxLength ? value[..maxLength] : value;
        }

        /// <summary>
        /// Impersonation is always logged, whatever the setting says: it is roughly ten rows a day and
        /// it is the compliance anchor. A switch that silently disables it will eventually be flipped.
        /// </summary>
        protected virtual async Task<bool> ShouldLog(UserSignInAttemptEvent message)
        {
            if (message.SignInType == SignInType.Impersonation ||
                message.SignInType == SignInType.ImpersonationRevert)
            {
                return true;
            }

            return await _settingsManager.GetValueAsync<bool>(PlatformConstants.Settings.Security.SignInLogEnabled);
        }
    }
}
