using System;
using System.Linq;
using Microsoft.Practices.Unity.Mvc;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using VirtoCommerce.Foundation.AppConfig;
using VirtoCommerce.Web.Client.Modules;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Initial.Web.ModuleConfig), "Start")]

namespace Initial.Web
{
    
    public static class ModuleConfig
    {
        /// <summary>
        /// Loads asp.net modules.
        /// </summary>
        public static void Start()
        {
            if (AppConfigConfiguration.Instance.Setup.IsCompleted)
            {
                foreach (var module in AppConfigConfiguration.Instance.AvailableModules.Cast<ModuleConfigurationElement>())
                {
                    try
                    {
                        var type = Type.GetType(module.Type);
                        DynamicModuleUtility.RegisterModule(type);
                    }
                    catch
                    {
                        //Skip module on error if config is wrong
                    }
                  
                }
            }
        }
    }
}