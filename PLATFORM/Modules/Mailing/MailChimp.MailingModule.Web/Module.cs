using MailChimp.MailingModule.Web.Controllers.Api;
using MailChimp.MailingModule.Web.Managers;
using MailChimp.MailingModule.Web.Services;
using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace MailChimp.MailingModule.Web
{
    public class Module : IModule
    {
        private const string _accessTokenPropertyName = "MailChimp.Mailing.Credentials.AccessToken";
        private const string _dataCenterPropertyName = "MailChimp.Mailing.Credentials.DataCenter";
        private const string _subscribersListIdPropertyName = "MailChimp.Mailing.SubscribersListId";

        private readonly IUnityContainer _container;
        public Module(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            var settingsManager = _container.Resolve<ISettingsManager>();

            var mailChimpAccessToken = settingsManager.GetValue(_accessTokenPropertyName, string.Empty);
            var mailChimpDataCenter = settingsManager.GetValue(_dataCenterPropertyName, string.Empty);
            var mailChimpListId = settingsManager.GetValue(_subscribersListIdPropertyName, string.Empty);

            var mailChimpCode = settingsManager.GetValue("MailChimp.Mailing.Code", string.Empty);
            var mailChimpDescription = settingsManager.GetValue("MailChimp.Mailing.Description", string.Empty);
            var mailChimpLogoUrl = settingsManager.GetValue("MailChimp.Mailing.LogoUrl", string.Empty);


            var mailChimpMailing = new MailChimpMailingImpl(mailChimpAccessToken, mailChimpDataCenter, mailChimpListId, mailChimpCode, mailChimpDescription, mailChimpLogoUrl);

            #region Mailing manager
            _container.RegisterInstance<IMailingManager>(new InMemoryMailingManagerImpl());
            #endregion

            var mailingManager = _container.Resolve<IMailingManager>();
            mailingManager.RegisterMailing(mailChimpMailing);

            _container.RegisterType<MailChimpController>
                (new InjectionConstructor(
                    mailChimpMailing));

            _container.RegisterType<MCAuthorizationController>
                (new InjectionConstructor(
                    settingsManager));
        }
    }
}
