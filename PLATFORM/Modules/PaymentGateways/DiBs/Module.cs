﻿using DiBs.Managers;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace DiBs
{
    public class Module : ModuleBase
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public override void PostInitialize()
        {
            var settings = _container.Resolve<ISettingsManager>().GetModuleSettings("DiBs");
            
            Func<DibsPaymentMethod> dibsPaymentMethodFactory = () => new DibsPaymentMethod("DIBS")
            {
                Name = "DIBS payment gateway",
                Description = "DIBS payment gateway integration",
                LogoUrl = "http://tech.dibspayment.com/sites/tech/files/pictures/LOGO/DIBS/PNG/DIBS_logo_blue_RGB.png",
                Settings = settings,
                IsActive = false
            };

            _container.Resolve<IPaymentMethodsService>().RegisterPaymentMethod(dibsPaymentMethodFactory);
        }
        
        #endregion
    }
}