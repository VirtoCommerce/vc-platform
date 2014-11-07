using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using VirtoCommerce.Framework.Core.Infrastructure;
using VirtoCommerce.Framework.Data.Extensions;
namespace VirtoCommerce.Framework.Data.Infrastructure
{
	public abstract class EFRepositoryBase : DbContext, IRepository, IUnitOfWork
	{
		private IUnitOfWork _unitOfWork;

		protected EFRepositoryBase(string nameOrConnectionString)
			: this(nameOrConnectionString, null)
		{
		}

		protected EFRepositoryBase(string nameOrConnectionString, IUnitOfWork unitOfWork)
			: base(nameOrConnectionString)
		{
			_unitOfWork = unitOfWork;

			var auditingUnitOfWork = unitOfWork as AuditUnitOfWork;
			if (auditingUnitOfWork != null)
				auditingUnitOfWork.ObserveContext(this);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			if (modelBuilder == null)
				throw new ArgumentNullException("modelBuilder");

			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			base.OnModelCreating(modelBuilder);
		}

		protected static void RegisterEntity<T>(DbModelBuilder modelBuilder)
			where T : Entity
		{
			if (modelBuilder == null)
				throw new ArgumentNullException("modelBuilder");

			modelBuilder.Entity<T>()
				.HasKey(e => e.Id)
				.Property(e => e.Id)
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
		}

		protected static void RegisterEntity<T>(DbModelBuilder modelBuilder, string tablePrefix)
			where T : Entity
		{
			if (modelBuilder == null)
				throw new ArgumentNullException("modelBuilder");

			var tableName = string.Join("_", tablePrefix, typeof(T).Name);

			modelBuilder.Entity<T>()
				.Map(e =>
				{
					e.MapInheritedProperties();
					e.ToTable(tableName);
				})
				.HasKey(e => e.Id)
				.Property(e => e.Id)
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
		}

		#region IRepository Members

		public IUnitOfWork UnitOfWork
		{
			get
			{
				return _unitOfWork ?? this;
			}
		}

		public void Attach<T>(T item) where T : class
		{
			this.Set(item.GetType()).Attach(item);
		}

		public void Add<T>(T item) where T : class
		{
			this.Set(item.GetType()).Add(item);
		}

		public void Update<T>(T item) where T : class
		{
			this.Set(item.GetType()).Attach(item);
			Entry(item).State = EntityState.Modified;
		}

		public void Remove<T>(T item) where T : class
		{
			this.Set(item.GetType()).Remove(item);
		}

		public IQueryable<T> GetAsQueryable<T>() where T : class
		{
			return this.Set<T>();
		}

		#endregion

		#region IUnitOfWork Members

		public virtual void Commit()
		{
			try
			{
				SaveChanges();
			}
			catch(Exception ex)
			{
				ex.ThrowFaultException();
			}
		}

		#endregion

	}
}
