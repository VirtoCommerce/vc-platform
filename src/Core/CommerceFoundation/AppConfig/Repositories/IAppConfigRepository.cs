using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.Foundation.AppConfig.Repositories
{
	public interface IAppConfigRepository : IRepository
	{
		IQueryable<Setting> Settings { get; }
		IQueryable<Sequence> Sequences { get; }
		IQueryable<SystemJob> SystemJobs { get; }
		IQueryable<SystemJobLogEntry> SystemJobLogEntries { get; }
		IQueryable<TaskSchedule> TaskSchedules { get; }
		IQueryable<Statistic> Statistics { get; }
		IQueryable<EmailTemplate> EmailTemplates { get; }
		IQueryable<EmailTemplateLanguage> EmailTemplateLanguages { get; }
		IQueryable<DisplayTemplateMapping> DisplayTemplateMappings { get; }
		IQueryable<ObjectLock> ObjectLocks { get; }
		IQueryable<Localization> Localizations { get; }
        IQueryable<SeoUrlKeyword> SeoUrlKeywords { get; } 
	}
}
