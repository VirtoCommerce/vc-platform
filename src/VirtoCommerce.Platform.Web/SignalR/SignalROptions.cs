using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Web.SignalR
{
    public class SignalROptions
    {
        public bool ScalabilityEnabled { get; set; }
        public SignalRScalabilityType ScalabilityType { get; set; }
        public AzureSignalRServiceOptions AzureSignalRService { get; set; } = new AzureSignalRServiceOptions();
    }



    public enum SignalRScalabilityType
    {
        AzureSignalRService,
        RedisBackplane
    }
}
