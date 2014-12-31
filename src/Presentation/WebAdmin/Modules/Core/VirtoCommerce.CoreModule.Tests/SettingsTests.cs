using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.CoreModule.Web.Settings;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Framework.Web.Settings;

namespace VirtoCommerce.CoreModule.Tests
{
	[TestClass]
	public class SettingsTests
	{
		[TestMethod]
		public void TestSettings()
		{
			const string moduleId = "VirtoCommerce.Core.Test";
			const string settingName = "VirtoCommerce.Core.Test.General.String";
			const string settingValue1 = "123";
			const string settingValue2 = "456";

			var manager = GetSettingsManager();

			var modules = manager.GetModules();
			Assert.IsNotNull(modules);

			var module = modules.FirstOrDefault(m => m.Id == moduleId);
			Assert.IsNotNull(module);

			var settings = manager.GetSettings(module.Id);
			Assert.IsNotNull(settings);

			var setting = settings.FirstOrDefault(s => s.Name == settingName);
			Assert.IsNotNull(setting);

			setting.Value = settingValue1;
			manager.SaveSettings(settings);

			var value1 = manager.GetValue(settingName, string.Empty);
			Assert.AreEqual(value1, settingValue1);

			manager.SetValue(settingName, settingValue2);

			var value2 = manager.GetValue(settingName, string.Empty);
			Assert.AreEqual(value2, settingValue2);

			manager.SetValue<string>(settingName, null);
		}


		private static ISettingsManager GetSettingsManager()
		{
			var modulesPath = Path.GetFullPath(@"..\..\modules");
			return new SettingsManager(modulesPath, () => new EFAppConfigRepository("VirtoCommerce"));
		}
	}
}
