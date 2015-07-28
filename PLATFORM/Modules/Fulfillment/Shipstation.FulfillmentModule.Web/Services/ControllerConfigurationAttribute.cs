using System;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;

namespace Shipstation.FulfillmentModule.Web.Services
{
    public class ControllerConfigAttribute : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings controllerSettings,
                               HttpControllerDescriptor controllerDescriptor)
        {
            var xmlFormater = new XmlMediaTypeFormatter { UseXmlSerializer = true };
            
            controllerSettings.Formatters.Clear();
            controllerSettings.Formatters.Add(xmlFormater);
        }
    }
}