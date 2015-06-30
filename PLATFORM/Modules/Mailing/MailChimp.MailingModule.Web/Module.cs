using System.Drawing.Text;
using MailChimp.MailingModule.Web.Controllers.Api;
using MailChimp.MailingModule.Web.Services;
using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace MailChimp.MailingModule.Web
{
    public class Module : IModule
    {
        private const string accessTokenPropertyName = "MailChimp.Mailing.Credentials.AccessToken";
        private const string dataCenterPropertyName = "MailChimp.Mailing.Credentials.DataCenter";
        private const string subscribersListIdPropertyName = "MailChimp.Mailing.SubscribersListId";

        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
        }

        public void Initialize()
        {
            var settingsManager = _container.Resolve<ISettingsManager>();
            
            var mailChimpCode = settingsManager.GetValue("MailChimp.Mailing.Code", string.Empty);
            var mailChimpDescription = settingsManager.GetValue("MailChimp.Mailing.Description", string.Empty);
            var mailChimpLogoUrl = settingsManager.GetValue("MailChimp.Mailing.LogoUrl", string.Empty);


            var mailChimpMailing = new MailChimpMailingSettings(settingsManager, accessTokenPropertyName, dataCenterPropertyName, subscribersListIdPropertyName, mailChimpCode, mailChimpDescription, mailChimpLogoUrl);
            
            _container.RegisterType<MailChimpController>
                (new InjectionConstructor(
                    mailChimpMailing));

            _container.RegisterType<MCAuthorizationController>
                (new InjectionConstructor(
                    settingsManager));
        }

        public void PostInitialize()
        {
        }

        #endregion
    }
}
