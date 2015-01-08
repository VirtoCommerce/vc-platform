using System;
using System.Linq;
using Microsoft.Practices.Unity.Mvc;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using VirtoCommerce.ApiWebClient.Configuration.Application;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PublicWebApp.ModuleConfig), "Start")]

namespace PublicWebApp
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