using System;
using System.Linq;

using Microsoft.WindowsAzure.ServiceRuntime;
using VirtoCommerce.Foundation.Data.Azure.Common;
namespace VirtoCommerce.Web
{
    public class WebRole : BaseWebRole
    {
        public override bool OnStart()
        {
            if (RoleEnvironment.IsAvailable && !RoleEnvironment.IsEmulated)
            {
                RoleEnvironment.Changing += OnRoleEnvironmentChanging;
            }
            return base.OnStart();
        }

        /// <summary>
        /// Force restart of the instance.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (e.Changes.Any(o => o is RoleEnvironmentChange))
                e.Cancel = true;
        }
    }

    /* The version below allows using IP rules engine, simply include the IPRule package
     * Install-Package WindowsAzure.IPAddressRules
    using WindowsAzure.IPAddressRules;
    public class WebRole : BaseWebRole
    {
        private IPAddressRulesManager ruleManager;

        public override bool OnStart()
        {
            if (RoleEnvironment.IsAvailable && !RoleEnvironment.IsEmulated)
            {
                RoleEnvironment.Changing += OnRoleEnvironmentChanging;
                ConfigureIPAddressRestrictions();
            }
            return base.OnStart();
        }

        private void ConfigureIPAddressRestrictions()
        {
            if (ruleManager == null)
                ruleManager = new IPAddressRulesManager();


            // Reset everything.
            ruleManager.ResetDisabledRules();
            ruleManager.DeleteRules();

            // Apply settings.
            if (IPAddressRulesConfiguration.IsEnabled())
                ruleManager.ApplySettings(IPAddressRulesConfiguration.GetSettings());
        }

        /// <summary>
        /// Force restart of the instance.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (e.Changes.Any(o => o is RoleEnvironmentChange))
                e.Cancel = true;
        }
    }
     */

}