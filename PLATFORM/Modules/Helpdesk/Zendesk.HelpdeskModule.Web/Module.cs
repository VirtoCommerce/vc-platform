
using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using Zendesk.HelpdeskModule.Web.Controllers.Api;
using Zendesk.HelpdeskModule.Web.Services;

namespace Zendesk.HelpdeskModule.Web
{
    public class Module : ModuleBase
    {
        private const string _accessTokenPropertyName = "Zendesk.Helpdesk.Credentials.AccessToken";
        private const string _customerEmailPropertyName = "Zendesk.Helpdesk.Credentials.Email";
        private const string _subdomainPropertyName = "Zendesk.Helpdesk.Subdomain";

        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        public override void Initialize()
        {
            base.Initialize();

            var settingsManager = _container.Resolve<ISettingsManager>();
            
            var zendeskHelpdesk = new ZendeskHelpdeskSettings(settingsManager, _accessTokenPropertyName, _subdomainPropertyName, _customerEmailPropertyName);
            
            _container.RegisterInstance<IHelpdeskSettings>(zendeskHelpdesk);
            _container.RegisterType<ZendeskController>();
            _container.RegisterType<ZDAuthorizationController>();
        }
    }
}