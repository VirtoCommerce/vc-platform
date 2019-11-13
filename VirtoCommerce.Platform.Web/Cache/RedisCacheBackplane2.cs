using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CacheManager.Core;
using CacheManager.Core.Internal;
using CacheManager.Core.Logging;
using CacheManager.Redis;
using static CacheManager.Core.Utility.Guard;
using StackRedis = StackExchange.Redis;

namespace VirtoCommerce.Platform.Web.Cache
{
    /// <summary>
    /// We were forced to copy this code because this class marked as sealed, however we need to disable raising Changed event 
    /// to prevent eviction from cache already exist values 
    /// </summary>
    public sealed class RedisCacheBackplane2 : CacheBackplane
    {
        private readonly string channelName;
        private readonly string identifier;
        private readonly ILogger logger;
        private readonly RedisConnectionManager2 connection;
        private HashSet<string> messages = new HashSet<string>();
        private object messageLock = new object();
        private int skippedMessages = 0;
        private bool sending = false;

        public RedisCacheBackplane2(CacheManagerConfiguration configuration, ILoggerFactory loggerFactory)
            : base(configuration)
        {
            NotNull(configuration, nameof(configuration));
            NotNull(loggerFactory, nameof(loggerFactory));

            this.logger = loggerFactory.CreateLogger(this);
            this.channelName = configuration.BackplaneChannelName ?? "CacheManagerBackplane";
            this.identifier = Guid.NewGuid().ToString();

            var cfg = RedisConfigurations.GetConfiguration(this.ConfigurationKey);
            this.connection = new RedisConnectionManager2(
                cfg,
                loggerFactory);

            RetryHelper2.Retry(() => this.Subscribe(), configuration.RetryTimeout, configuration.MaxRetries, this.logger);
        }

        public override void NotifyChange(string key)
        {
            //this.PublishMessage(BackplaneMessage.ForChanged(this.identifier, key));
        }

        public override void NotifyChange(string key, string region)
        {
            //this.PublishMessage(BackplaneMessage.ForChanged(this.identifier, key, region));
        }

        public override void NotifyClear()
        {
            this.PublishMessage(BackplaneMessage.ForClear(this.identifier));
        }

        public override void NotifyClearRegion(string region)
        {
            this.PublishMessage(BackplaneMessage.ForClearRegion(this.identifier, region));
        }

        public override void NotifyRemove(string key)
        {
            this.PublishMessage(BackplaneMessage.ForRemoved(this.identifier, key));
        }

        public override void NotifyRemove(string key, string region)
        {
            this.PublishMessage(BackplaneMessage.ForRemoved(this.identifier, key, region));
        }

        protected override void Dispose(bool managed)
        {
            if (managed)
            {
                this.connection.Subscriber.Unsubscribe(this.channelName);
            }

            base.Dispose(managed);
        }

        private void Publish(string message)
        {
            this.connection.Subscriber.Publish(this.channelName, message, StackRedis.CommandFlags.HighPriority);
        }

        private void PublishMessage(BackplaneMessage message)
        {
            var msg = message.Serialize();

            lock (this.messageLock)
            {
                if (message.Action == BackplaneAction.Clear)
                {
                    Interlocked.Exchange(ref this.skippedMessages, this.messages.Count);
                    this.messages.Clear();
                }

                if (!this.messages.Add(msg))
                {
                    Interlocked.Increment(ref this.skippedMessages);
                    if (this.logger.IsEnabled(LogLevel.Trace))
                    {
                        this.logger.LogTrace("Skipped duplicate message: {0}.", msg);
                    }
                }

                this.SendMessages();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "No other way")]
        private void SendMessages()
        {
            if (this.sending || this.messages == null || this.messages.Count == 0)
            {
                return;
            }

            Task.Factory.StartNew(
                (obj) =>
                {
                    if (this.sending || this.messages == null || this.messages.Count == 0)
                    {
                        return;
                    }

                    this.sending = true;
#if !NET40
                    Task.Delay(10).Wait();
#endif
                    string msgs = string.Empty;
                    lock (this.messageLock)
                    {
                        if (this.messages != null && this.messages.Count > 0)
                        {
                            msgs = string.Join(",", this.messages);

                            if (this.logger.IsEnabled(LogLevel.Debug))
                            {
                                this.logger.LogDebug("Backplane is sending {0} messages ({1} skipped).", this.messages.Count, this.skippedMessages);
                            }

                            this.skippedMessages = 0;
                            this.messages.Clear();
                        }

                        try
                        {
                            if (msgs.Length > 0)
                            {
                                this.Publish(msgs);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogError(ex, "Error occurred sending backplane messages.");
                        }

                        this.sending = false;
                    }
#if NET40
                },
                this,
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.Default)
                .ConfigureAwait(false);
#else
                },
                this,
                CancellationToken.None,
                TaskCreationOptions.DenyChildAttach,
                TaskScheduler.Default)
                .ConfigureAwait(false);
#endif
        }

        private void Subscribe()
        {
            this.connection.Subscriber.Subscribe(
                this.channelName,
                (channel, msg) =>
                {
                    var fullMessage = ((string)msg).Split(',')
                        .Where(s => !s.StartsWith(this.identifier, StringComparison.Ordinal))
                        .ToArray();

                    if (fullMessage.Length == 0)
                    {
                        // no messages for this instance
                        return;
                    }

                    if (this.logger.IsEnabled(LogLevel.Information))
                    {
                        this.logger.LogInfo("Backplane got notified with {0} new messages.", fullMessage.Length);
                    }

                    foreach (var messageStr in fullMessage)
                    {
                        var message = BackplaneMessage.Deserialize(messageStr);

                        switch (message.Action)
                        {
                            case BackplaneAction.Clear:
                                this.TriggerCleared();
                                break;

                            case BackplaneAction.ClearRegion:
                                this.TriggerClearedRegion(message.Region);
                                break;

                            //case BackplaneAction.Changed:
                            //    if (string.IsNullOrWhiteSpace(message.Region))
                            //    {
                            //        this.TriggerChanged(message.Key);
                            //    }
                            //    else
                            //    {
                            //        this.TriggerChanged(message.Key, message.Region);
                            //    }
                            //    break;

                            case BackplaneAction.Removed:
                                if (string.IsNullOrWhiteSpace(message.Region))
                                {
                                    this.TriggerRemoved(message.Key);
                                }
                                else
                                {
                                    this.TriggerRemoved(message.Key, message.Region);
                                }
                                break;
                        }
                    }
                },
                StackRedis.CommandFlags.FireAndForget);
        }
    }
}