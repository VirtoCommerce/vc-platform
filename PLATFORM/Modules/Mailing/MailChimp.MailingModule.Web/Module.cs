using MailChimp.MailingModule.Web.Controllers;
using MailChimp.MailingModule.Web.Managers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.MailChimp.Mailing.Services;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Framework.Web.Settings;

namespace MailChimp.MailingModule.Web
{
	public class Module : IModule
	{
		private readonly IUnityContainer _container;
		public Module(IUnityContainer container)
		{
			_container = container;
		}

		public void Initialize()
		{
			var settingsManager = _container.Resolve<ISettingsManager>();

			var mailChimpApiKey = settingsManager.GetValue("MailChimp.Mailing.Credentials.ApiKey", string.Empty);

            var mailChimpCode = settingsManager.GetValue("MailChimp.Mailing.Code", string.Empty);
            var mailChimpDescription = settingsManager.GetValue("MailChimp.Mailing.Description", string.Empty);
            var mailChimpLogoUrl = settingsManager.GetValue("MailChimp.Mailing.LogoUrl", string.Empty);


            var mailChimpMailing = new MailChimpMailingImpl(mailChimpApiKey, mailChimpCode, mailChimpDescription, mailChimpLogoUrl);

            #region Mailing manager
            _container.RegisterInstance<IMailingManager>(new InMemoryMailingManagerImpl());
            #endregion

			var mailingManager = _container.Resolve<IMailingManager>();
            mailingManager.RegisterMailing(mailChimpMailing);

            _container.RegisterType<MailChimpController>(new InjectionConstructor(mailChimpMailing, mailChimpApiKey));
		}
	}
}