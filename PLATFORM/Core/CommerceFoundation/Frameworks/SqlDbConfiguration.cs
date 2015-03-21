using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Runtime.Remoting.Messaging;
using VirtoCommerce.Foundation.AppConfig;

namespace VirtoCommerce.Foundation.Frameworks
{
    public class SqlDbConfiguration : DbConfiguration
    {
        public const string SqlAzureExecutionStrategy = "SqlAzureExecutionStrategy";

        public class SuspensionFlag : IDisposable
        {
            public SuspensionFlag(bool suspended = true)
            {
                SuspendExecutionStrategy = suspended;
            }
            public void Dispose()
            {
                SuspendExecutionStrategy = false;
            }
        }

        public static void Register()
        {
            SetConfiguration(new SqlDbConfiguration());
        }

        /// <summary>
        /// Property can be used with 'using' statement. It is safe to use it multiple time in nested blocks.
        /// </summary>
        public static SuspensionFlag ExecutionStrategySuspension
        {
            get
            {
                // No need to suspend for nested block
                return SuspendExecutionStrategy?null:new SuspensionFlag();
            }
        }

        public static bool SuspendExecutionStrategy
        {
            get
            {
                return (bool?)CallContext.LogicalGetData("SuspendExecutionStrategy") ?? false;
            }
            set
            {
                CallContext.LogicalSetData("SuspendExecutionStrategy", value);
            }
        } 

        public SqlDbConfiguration()
        {
            if (AppConfigConfiguration.Instance == null)
            {
                return;
            }

            foreach (StrategyConfigurationElement strategy in AppConfigConfiguration.Instance.SqlExecutionStrategies)
            {
                var strategyType = Type.GetType(strategy.StrategyType);
                if (strategyType != null)
                {
                    var maxDelay = strategy.MaxDelay.Value;
                    IDbExecutionStrategy strategyObj = (IDbExecutionStrategy) Activator.CreateInstance(strategyType);

                    if (string.IsNullOrWhiteSpace(strategy.ServerName))
                    {
                        SetExecutionStrategy(strategy.ProviderName,
                            delegate
                            {
                                return SuspendExecutionStrategy
                                    ? (IDbExecutionStrategy) new DefaultExecutionStrategy()
                                    : strategyObj;
                            });
                    }
                    else
                    {
                        SetExecutionStrategy(strategy.ProviderName, 
                            () => SuspendExecutionStrategy
                                ?(IDbExecutionStrategy)new DefaultExecutionStrategy()
                                :strategyObj, strategy.ServerName);
                    }
                }
            }
        }

        public SqlDbConfiguration(string strategyTypeName)
        {
            IDbExecutionStrategy strategyObj;

            if (strategyTypeName == SqlAzureExecutionStrategy)
            {
                strategyObj = new SqlAzureExecutionStrategy();
            }
            else
            {
                var strategyType = Type.GetType(strategyTypeName);
                strategyObj = (IDbExecutionStrategy)Activator.CreateInstance(strategyType);
            }

            SetExecutionStrategy("System.Data.SqlClient", 
                () => SuspendExecutionStrategy
                        ? (IDbExecutionStrategy)new DefaultExecutionStrategy()
                        : strategyObj);
        }
    }
}
