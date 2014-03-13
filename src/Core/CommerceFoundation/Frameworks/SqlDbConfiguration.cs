using System;
using System.Activities.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using VirtoCommerce.Foundation.AppConfig;

namespace VirtoCommerce.Foundation.Frameworks
{
    public class SqlDbConfiguration : DbConfiguration
    {
        public const string SqlAzureExecutionStrategy = "SqlAzureExecutionStrategy";

        public static void Register()
        {
            if (AppConfigConfiguration.Instance != null &&  
                AppConfigConfiguration.Instance.SqlExecutionStrategies.Count > 0)
            {
                SetConfiguration(new SqlDbConfiguration());
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
                        SetExecutionStrategy(strategy.ProviderName, () => strategyObj);
                    }
                    else
                    {
                        SetExecutionStrategy(strategy.ProviderName, () => strategyObj, strategy.ServerName);
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

            SetExecutionStrategy("System.Data.SqlClient", () => strategyObj);
        }
    }
}
