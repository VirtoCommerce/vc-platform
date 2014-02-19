using System;
using System.Linq;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Security.Services;
using VirtoCommerce.Foundation.AppConfig;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Data.AppConfig
{
	public class DSAppConfigClient : DSClientBase, IAppConfigRepository
	{
		[InjectionConstructor]
		public DSAppConfigClient(IAppConfigEntityFactory factory, ISecurityTokenInjector tokenInjector, IServiceConnectionFactory connFactory)
			: base(connFactory.GetConnectionString(AppConfigConfiguration.Instance.Connection.DataServiceUri), factory, tokenInjector)
		{
		}

		public DSAppConfigClient(Uri serviceUri, IAppConfigEntityFactory factory, ISecurityTokenInjector tokenInjector)
			: base(serviceUri, factory, tokenInjector)
		{
		}


		#region IAppConfigRepository Members

		public IQueryable<Setting> Settings
		{
			get { return GetAsQueryable<Setting>(); }
		}

	    public IQueryable<Sequence> Sequences
	    {
            get { return GetAsQueryable<Sequence>(); }
	    }

	    public IQueryable<SystemJob> SystemJobs
		{
			get { return GetAsQueryable<SystemJob>(); }
		}

		public IQueryable<SystemJobLogEntry> SystemJobLogEntries
		{
			get { return GetAsQueryable<SystemJobLogEntry>(); }
		}

		public IQueryable<TaskSchedule> TaskSchedules
        {
            get { return GetAsQueryable<TaskSchedule>(); }
        }

        public IQueryable<Statistic> Statistics
        {
            get { return GetAsQueryable<Statistic>(); }
        }

        public IQueryable<EmailTemplate> EmailTemplates
        {
            get { return GetAsQueryable<EmailTemplate>(); }
        }

        public IQueryable<EmailTemplateLanguage> EmailTemplateLanguages
        {
            get { return GetAsQueryable<EmailTemplateLanguage>(); }
        }

		public IQueryable<DisplayTemplateMapping> DisplayTemplateMappings
		{
			get { return GetAsQueryable<DisplayTemplateMapping>(); }
		}

        public IQueryable<ObjectLock> ObjectLocks
        {
            get { return GetAsQueryable<ObjectLock>(); }
        }

		public IQueryable<Localization> Localizations
		{
			get { return GetAsQueryable<Localization>(); }
		}

	    public IQueryable<SeoUrlKeyword> SeoUrlKeywords
	    {
	        get { return GetAsQueryable<SeoUrlKeyword>(); }
	    }

	    #endregion
       
    }
}
