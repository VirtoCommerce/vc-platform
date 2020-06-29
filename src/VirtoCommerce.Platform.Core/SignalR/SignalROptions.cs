namespace VirtoCommerce.Platform.Core.SignalR
{
    public class SignalROptions
    {
        public SignalRScalabilityProvider ScalabilityProvider { get; set; }

        public AzureSignalROptions AzureSignalRService { get; set; }

        public RedisBackplaneSignalROptions RedisBackplane { get; set; }
    }
}
