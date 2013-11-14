namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
    public interface IAppSettings
    {
        object this[string propertyName] { get; set; }

        void Save();
    }
    public interface IGlobalConfigService
    {
        void Update(string settingName, object value);
        object Get(string settingName);
    }


}
