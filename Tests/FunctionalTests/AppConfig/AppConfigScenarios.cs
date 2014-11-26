using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using FunctionalTests.TestHelpers;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.AppConfig.Migrations;
using VirtoCommerce.Foundation.Data.AppConfig.Services;
using VirtoCommerce.PowerShell.DatabaseSetup;
using Xunit;

namespace FunctionalTests.AppConfig
{
	[Variant(RepositoryProvider.EntityFramework)]
	[Variant(RepositoryProvider.DataService)]
	public class SecurityScenarios : FunctionalTestBase, IDisposable
	{
		#region Infrastructure/setup

		private readonly string _databaseName;
		private readonly object _previousDataDirectory;

		public SecurityScenarios()
		{
			_previousDataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory");
            AppDomain.CurrentDomain.SetData("DataDirectory", TempPath);
			_databaseName = "AppConfigTest";
		}

		public void Dispose()
		{
			try
			{
				// Ensure LocalDb databases are deleted after use so that LocalDb doesn't throw if
				// the temp location in which they are stored is later cleaned.
				using (var context = new EFAppConfigRepository(_databaseName))
				{
					context.Database.Delete();
				}
			}
			finally
			{
				AppDomain.CurrentDomain.SetData("DataDirectory", _previousDataDirectory);
			}
		}

		#endregion

		#region Tests

		//[Fact]
		//public void Can_create_security_graph()
		//{

		//}

		#endregion

		#region Real Scenarios Tests

		[Fact]
		public void Can_add_appsettings()
		{

			var setting = new Setting {Name = "Currencies", SettingValueType = "ShortText", IsMultiValue = true, IsSystem = true};
			var id = setting.SettingId;
			setting.SettingValues.Add(new SettingValue() { ValueType = "ShortText", ShortTextValue = "USD", SettingId = id });
			setting.SettingValues.Add(new SettingValue() { ValueType = "ShortText", ShortTextValue = "EUR", SettingId = id });

			var client = GetRepository();
			client.Add(setting);
			client.UnitOfWork.Commit();

			var checkRepository = GetRepository();
			setting = checkRepository.Settings.Where(x => x.Name == "Currencies").SingleOrDefault();
			Assert.NotNull(setting);
		}

		[Fact]
		public void Can_Add_Localization()
		{
			var client = GetRepository();
			var localizationItem = new Localization {LanguageCode = "EN", Name = "1", Value = "Test", Category = "1", };
			var items = client.Localizations.ToList();
			items.Add(localizationItem);
			client.Add(localizationItem);
			client.UnitOfWork.Commit();

			var checkRepository = GetRepository();
			localizationItem = checkRepository.Localizations.Where(x => x.Name == "1").SingleOrDefault();
			Assert.NotNull(localizationItem);

		}

        [Fact(Skip = "IdentitySequence Table needs to be created manually CREATE TABLE [dbo].[IdentitySequence]([Id] [int] IDENTITY(1,1) NOT NULL) ON [PRIMARY]")]
        public void Can_generate_sequence()
        {
            var client = new EFAppConfigRepository("VirtoCommerce");
            //var client = GetRepository();
            var sequence = new DbIdentitySequenceService(client);
            var result = sequence.GetNext("test");
            Assert.NotNull(result);
        }

		#endregion

		#region Helper Methods

		private IAppConfigRepository GetRepository()
		{
			EnsureDatabaseInitialized(() => new EFAppConfigRepository(_databaseName), () => Database.SetInitializer(new SetupMigrateDatabaseToLatestVersion<EFAppConfigRepository, Configuration>()));
			var retVal = new EFAppConfigRepository(_databaseName);
			return retVal;
		}

		#endregion
	
	}
}
