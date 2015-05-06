
using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using Zendesk.HelpdeskModule.Web.Controllers.Api;
using Zendesk.HelpdeskModule.Web.Managers;
using Zendesk.HelpdeskModule.Web.Services;

namespace Zendesk.HelpdeskModule.Web
{
    public class Module : ModuleBase
    {
        private const string _accessTokenPropertyName = "Zendesk.Helpdesk.Credentials.AccessToken";

        private readonly IUnityContainer _container;
        public Module(IUnityContainer container)
        {
            _container = container;
        }

        public override void Initialize()
        {
            var settingsManager = _container.Resolve<ISettingsManager>();

            var zendeskAccessToken = settingsManager.GetValue(_accessTokenPropertyName, string.Empty);

            var zendeskCode = settingsManager.GetValue("Zendesk.Helpdesk.Code", string.Empty);
            var zendeskDescription = settingsManager.GetValue("Zendesk.Helpdesk.Description", string.Empty);
            var zendeskLogoUrl = settingsManager.GetValue("Zendesk.Helpdesk.LogoUrl", string.Empty);


            var zendeskHelpdesk = new ZendeskHelpdeskImpl(zendeskAccessToken, zendeskCode, zendeskDescription, zendeskLogoUrl);

            #region Mailing manager
            _container.RegisterInstance<IHelpdeskManager>(new InMemoryHelpdeskManager());
            #endregion

            var zendeskManager = _container.Resolve<IHelpdeskManager>();
            zendeskManager.RegisterHelpdesk(zendeskHelpdesk);

            _container.RegisterType<ZendeskController>
                (new InjectionConstructor(
                    zendeskHelpdesk));

            _container.RegisterType<ZDAuthorizationController>
                (new InjectionConstructor(
                    settingsManager));
        }
    }
}