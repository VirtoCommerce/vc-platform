using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Framework.Core.Infrastructure;

namespace VirtoCommerce.Framework.Data.Infrastructure
{
	public class AuditUnitOfWork : EFRepositoryBase
	{
		private EFRepositoryBase _surveyRepository;
		private readonly string[] _surveyEntities;
		private readonly string[] _surveyFields;

		public AuditUnitOfWork(string nameOrConnectionString, string[] surveyEntities = null, string[] surveyFields = null)
			: base(nameOrConnectionString)
		{
			_surveyEntities = surveyEntities;
			_surveyFields = surveyFields;

			Database.SetInitializer<AuditUnitOfWork>(null);
			Configuration.LazyLoadingEnabled = false;
		}

		public void ObserveContext(EFRepositoryBase context)
		{
			if (_surveyRepository != null)
			{
				throw new OperationCanceledException("Already observed");
			}

			_surveyRepository = context;
			_surveyRepository.Configuration.ProxyCreationEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			RegisterEntity<ActivityLog>(modelBuilder);
			modelBuilder.Entity<ActivityLog>().ToTable("ActivityLog");
			modelBuilder.Entity<ActivityLog>().Ignore(x => x.Tag);

			base.OnModelCreating(modelBuilder);
		}

		public IQueryable<ActivityLog> ActivityLogs
		{
			get { return GetAsQueryable<ActivityLog>(); }
		}

		public override void Commit()
		{
			var activityLogs = new List<ActivityLog>();

			var changedEntries = _surveyRepository.ChangeTracker.Entries().Where(entry => (entry.State == EntityState.Added) || (entry.State == EntityState.Modified) || (entry.State == EntityState.Deleted));

			if (changedEntries != null)
			{
				foreach (var entity in changedEntries)
				{
					if (_surveyEntities.Contains(entity.Entity.GetType().Name))
					{
						foreach (var activityLog in GetAuditRecordsForChange(entity))
						{
							activityLogs.Add(activityLog);
						}
					}
				}
			}

			_surveyRepository.SaveChanges();

			foreach (var activityLog in activityLogs)
			{
				activityLog.EntityIdentity = ((DbEntityEntry)(activityLog.Tag)).Property("Id").CurrentValue.ToString();

				this.Add(activityLog);
			}

			this.SaveChanges();
		}

		private List<ActivityLog> GetAuditRecordsForChange(DbEntityEntry dbEntry)
		{
			var result = new List<ActivityLog>();

			if (dbEntry.State == EntityState.Modified)
			{
				foreach (var propertyName in dbEntry.OriginalValues.PropertyNames)
				{
					if (!object.Equals(dbEntry.OriginalValues.GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName)) && _surveyFields.Contains(propertyName))
					{
						var activityLog = ConvertDbEntryToActivityLog(dbEntry);

						activityLog.FieldName = propertyName;
						activityLog.OldValue = dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? null : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString();
						activityLog.NewValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString();

						result.Add(activityLog);
					}
				}
			}

			return result;
		}

		private ActivityLog ConvertDbEntryToActivityLog(DbEntityEntry dbEntry)
		{
			string userId = Thread.CurrentPrincipal == null ? "unknown" : Thread.CurrentPrincipal.Identity.Name;

			return new ActivityLog
			{
				ActivityType = dbEntry.State.ToString(),
				Created = DateTime.UtcNow,
				EntityType = dbEntry.Entity.GetType().Name,
				Tag = dbEntry,
				UserName = userId
			};
		}
	}
}