using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.Foundation.AppConfig.Factories
{
	public class AppConfigEntityFactory : FactoryBase, IAppConfigEntityFactory
	{
		public AppConfigEntityFactory()
		{
			RegisterStorageType(typeof(Setting), "Setting");
			RegisterStorageType(typeof(SettingValue), "SettingValue");
			RegisterStorageType(typeof(SystemJob), "SystemJob");
            RegisterStorageType(typeof(TaskSchedule), "TaskSchedule");
            RegisterStorageType(typeof(Sequence), "Sequence");
			RegisterStorageType(typeof(JobParameter), "JobParameter");
			RegisterStorageType(typeof(SystemJobLogEntry), "SystemJobLogEntry");
		    RegisterStorageType(typeof (Statistic), "Statistic");
            RegisterStorageType(typeof(EmailTemplate),"EmailTemplate");
		    RegisterStorageType(typeof (EmailTemplateLanguage), "EmailTemplateLanguage");
			RegisterStorageType(typeof(DisplayTemplateMapping), "DisplayTemplateMapping");
			RegisterStorageType(typeof(Localization), "Localization");
			RegisterStorageType(typeof(SeoUrlKeyword), "SeoUrlKeyword");
		}
	}
}
