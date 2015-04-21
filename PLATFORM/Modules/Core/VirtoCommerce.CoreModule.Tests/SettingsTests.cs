using System.IO;
using System.Linq;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.CoreModule.Web.Settings;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using Xunit;

namespace VirtoCommerce.CoreModule.Tests
{
    public class SettingsTests
    {
        [Fact]
        public void CanLoadAndSaveArray()
        {
            const string settingName = "VirtoCommerce.Core.Test.General.IntegerArray";
            var emptyArray = new int[0];
            var settingValue1 = new[] { 22, 11 };
            var settingValue2 = new[] { 33, 22, 11 };
            var manager = GetSettingsManager();

            var value0 = manager.GetArray(settingName, emptyArray);
            Assert.NotNull(value0);

            manager.SetValue(settingName, settingValue1);
            var value1 = manager.GetArray(settingName, emptyArray);
            Assert.NotNull(value1);
            Assert.Equal(value1.OrderBy(v => v), settingValue1.OrderBy(v => v));

            manager.SetValue(settingName, settingValue2);
            var value2 = manager.GetArray(settingName, emptyArray);
            Assert.NotNull(value2);
            Assert.Equal(value2.OrderBy(v => v), settingValue2.OrderBy(v => v));

            manager.SetValue(settingName, settingValue1);
            var value3 = manager.GetArray(settingName, emptyArray);
            Assert.NotNull(value3);
            Assert.Equal(value3.OrderBy(v => v), settingValue1.OrderBy(v => v));

            manager.SetValue<int[]>(settingName, null);
            var value4 = manager.GetArray(settingName, emptyArray);
            Assert.NotNull(value4);
            Assert.Equal(value4, emptyArray);

            var value5 = manager.GetArray<int>(settingName, null);
            Assert.NotNull(value5);
            Assert.Equal(value5, emptyArray);
        }

        [Fact]
        public void TestSettings()
        {
            const string moduleId = "VirtoCommerce.Core.Test";
            const string settingName = "VirtoCommerce.Core.Test.General.String";
            const string settingValue1 = "123";
            const string settingValue2 = "456";

            var manager = GetSettingsManager();

            var modules = manager.GetModules();
            Assert.NotNull(modules);

            var module = modules.FirstOrDefault(m => m.Id == moduleId);
            Assert.NotNull(module);

            var settings = manager.GetSettings(module.Id);
            Assert.NotNull(settings);

            var setting = settings.FirstOrDefault(s => s.Name == settingName);
            Assert.NotNull(setting);

            setting.Value = settingValue1;
            manager.SaveSettings(settings);

            var value1 = manager.GetValue(settingName, string.Empty);
            Assert.Equal(value1, settingValue1);

            manager.SetValue(settingName, settingValue2);

            var value2 = manager.GetValue(settingName, string.Empty);
            Assert.Equal(value2, settingValue2);

            manager.SetValue<string>(settingName, null);
        }


        private static ISettingsManager GetSettingsManager()
        {
            var modulesPath = Path.GetFullPath(".");
            var manifestProvider = new ModuleManifestProvider(modulesPath) { ManifestFileName = "manifest.xml" };
            return new SettingsManager(manifestProvider, () => new EFAppConfigRepository("VirtoCommerce"), new HttpCacheRepository());
        }
    }
}
