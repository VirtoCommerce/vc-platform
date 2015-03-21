using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.Foundation.Data.Customers
{
	public class EFCustomerRepository : EFRepositoryBase, ICustomerRepository
	{
		public EFCustomerRepository()
		{
		}

		public EFCustomerRepository(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;
			Database.SetInitializer<EFCustomerRepository>(null);
		}

		[InjectionConstructor]
		public EFCustomerRepository(ICustomerEntityFactory entityFactory, IInterceptor[] interceptors = null)
			: this(CustomerConfiguration.Instance.Connection.SqlConnectionStringName, entityFactory, interceptors: interceptors)
		{
		}

		public EFCustomerRepository(string connectionStringName, ICustomerEntityFactory entityFactory, IInterceptor[] interceptors = null)
			: base(connectionStringName, factory: entityFactory, interceptors: interceptors)
		{
			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;
			Database.SetInitializer(new ValidateDatabaseInitializer<EFCustomerRepository>());
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			InheritanceMapping(modelBuilder);

			MapEntity<Address>(modelBuilder, toTable: "Address");
			MapEntity<Attachment>(modelBuilder, toTable: "Attachment");
			MapEntity<Case>(modelBuilder, toTable: "Case");
			MapEntity<CaseTemplate>(modelBuilder, toTable: "CaseTemplate");
			MapEntity<CaseTemplateProperty>(modelBuilder, toTable: "CaseTemplateProperty");
			MapEntity<CaseRule>(modelBuilder, toTable: "CaseRule");
			MapEntity<CaseAlert>(modelBuilder, toTable: "CaseAlert");
			MapEntity<CaseCC>(modelBuilder, toTable:"CaseCC");
			MapEntity<CasePropertySet>(modelBuilder, toTable: "CasePropertySet");
			MapEntity<CaseProperty>(modelBuilder, toTable: "CaseProperty");
			MapEntity<CasePropertyValue>(modelBuilder, toTable: "CasePropertyValue");
			MapEntity<ContactPropertyValue>(modelBuilder, toTable: "ContactPropertyValue");
			MapEntity<Label>(modelBuilder, toTable: "Label");
			MapEntity<Note>(modelBuilder, toTable: "Note");
			MapEntity<Phone>(modelBuilder, toTable: "Phone");
			MapEntity<Organization>(modelBuilder, toTable: "Organization");
			MapEntity<KnowledgeBaseArticle>(modelBuilder, toTable: "KnowledgeBaseArticle");
			MapEntity<KnowledgeBaseGroup>(modelBuilder, toTable: "KnowledgeBaseGroup");
			MapEntity<Contract>(modelBuilder, toTable: "Contract");

			base.OnModelCreating(modelBuilder);
		}

		private void InheritanceMapping(DbModelBuilder modelBuilder)
		{

			modelBuilder.Entity<Member>().Map(entity =>
				{
					entity.ToTable("Member");
				});
			modelBuilder.Entity<Contact>().Map(entity =>
				{
					entity.ToTable("Contact");
				});

			modelBuilder.Entity<CommunicationItem>().Map(entity =>
				{
					entity.ToTable("CommunicationItem");
				});
			modelBuilder.Entity<PhoneCallItem>().Map(entity =>
				{
					entity.ToTable("PhoneCallItem");
				});

			modelBuilder.Entity<EmailItem>().Map(entity =>
				{
					entity.ToTable("EmailItem");
				});
			modelBuilder.Entity<PublicReplyItem>().Map(entity =>
				{
					entity.ToTable("PublicReplyItem");
				});

			MapEntity<MemberRelation>(modelBuilder, toTable: "MemberRelation");

			#region Contact mappping

			modelBuilder.Entity<Member>()
				.HasMany(c => c.Emails)
				.WithRequired(e => e.Member);
			modelBuilder.Entity<Member>()
				.HasMany(c => c.Phones)
				.WithRequired(p => p.Member);
			modelBuilder.Entity<Member>()
				.HasMany(c => c.Contracts)
				.WithRequired(p => p.Member);
			modelBuilder.Entity<Member>()
				.HasMany(c => c.Addresses)
				.WithRequired(p => p.Member);
			modelBuilder.Entity<Contact>()
				.HasMany(c => c.ContactPropertyValues)
				.WithRequired(p => p.Contact);
			modelBuilder.Entity<Member>()
				.HasMany(c => c.Notes)
				.WithOptional(p => p.Member);
			modelBuilder.Entity<Member>()
				.HasMany(c => c.Labels)
				.WithOptional(p => p.Member);

			#endregion

			#region Case Mapping

			modelBuilder.Entity<Case>()
				.HasMany(c => c.CommunicationItems)
				.WithRequired(ci => ci.Case);
			modelBuilder.Entity<Case>()
				.HasMany(c => c.Notes)
				.WithOptional(n => n.Case);
			modelBuilder.Entity<Case>()
			  .HasMany(c => c.Labels)
			  .WithOptional(n => n.Case);
			modelBuilder.Entity<Case>()
				.HasMany(ci => ci.CaseCc)
				.WithRequired(ci => ci.Case);
			modelBuilder.Entity<Case>()
				.HasMany(ci => ci.CasePropertyValues)
				.WithRequired(ci => ci.Case);

			#endregion

			#region KnowledgeBase mapping

			modelBuilder.Entity<KnowledgeBaseArticle>()
				.HasMany(kba => kba.Attachments)
				.WithOptional(a => a.KnowledgeBaseArticle);

			modelBuilder.Entity<KnowledgeBaseGroup>()
				.HasMany(kbg => kbg.KnowledgeBaseArticles)
				.WithOptional(kba => kba.Group);

			modelBuilder.Entity<KnowledgeBaseGroup>()
				.HasOptional(kbg => kbg.Parent);

			#endregion

			modelBuilder.Entity<CommunicationItem>()
				.HasMany(ci => ci.Attachments)
				.WithOptional(ci => ci.CommunicationItem);

			modelBuilder.Entity<CasePropertySet>()
				.HasMany(ci => ci.CaseProperties)
				.WithRequired(ci => ci.CasePropertySet);

			modelBuilder.Entity<CaseRule>()
				.HasMany(ci => ci.Alerts)
				.WithRequired(ci => ci.CaseRule);

			modelBuilder.Entity<CaseTemplate>()
				.HasMany(ci => ci.CaseTemplateProperties)
				.WithRequired(ci => ci.CaseTemplate);

			modelBuilder.Entity<MemberRelation>()
				.HasRequired(m => m.Descendant)
				.WithMany(m => m.MemberRelations)
				.WillCascadeOnDelete(false);
		}

		#region ICustomerServiceRepository Members

		public IQueryable<Case> Cases
		{
			get { return GetAsQueryable<Case>(); }
		}

		public IQueryable<CaseTemplate> CaseTemplates
		{
			get { return GetAsQueryable<CaseTemplate>(); }
		}

		public IQueryable<CaseRule> CaseRules
		{
			get { return GetAsQueryable<CaseRule>(); }
		}

		public IQueryable<CaseAlert> CaseAlerts
		{
			get { return GetAsQueryable<CaseAlert>(); }
		}

		public IQueryable<CasePropertySet> CasePropertySets
		{
			get { return GetAsQueryable<CasePropertySet>(); }
		}

		public IQueryable<CasePropertyValue> CasePropertyValues
		{
			get { return GetAsQueryable<CasePropertyValue>(); }
		}

		public IQueryable<ContactPropertyValue> ContactPropertyValues
		{
			get { return GetAsQueryable<ContactPropertyValue>(); }
		}

		public IQueryable<CaseCC> CaseCcs
		{
			get { return GetAsQueryable<CaseCC>(); }
		}

		public IQueryable<Address> Addresses
		{
			get { return GetAsQueryable<Address>(); }
		}

		public IQueryable<Email> Emails
		{
			get { return GetAsQueryable<Email>(); }
		}

		public IQueryable<Label> Labels
		{
			get { return GetAsQueryable<Label>(); }
		}

		public IQueryable<Note> Notes
		{
			get { return GetAsQueryable<Note>(); }
		}

		public IQueryable<Phone> Phones
		{
			get { return GetAsQueryable<Phone>(); }
		}

		public IQueryable<Organization> Organizations
		{
			get { return GetAsQueryable<Organization>(); }
		}

		public IQueryable<KnowledgeBaseArticle> KnowledgeBaseArticles
		{
			get { return GetAsQueryable<KnowledgeBaseArticle>(); }
		}

		public IQueryable<KnowledgeBaseGroup> KnowledgeBaseGroups
		{
			get { return GetAsQueryable<KnowledgeBaseGroup>(); }
		}

		public IQueryable<Member> Members
		{
			get { return GetAsQueryable<Member>(); }
		}

		public IQueryable<CommunicationItem> CommunicationItems
		{
			get { return GetAsQueryable<CommunicationItem>(); }
		}

		public IQueryable<Attachment> Attachments
		{
			get { return GetAsQueryable<Attachment>(); }
		}

		public IQueryable<MemberRelation> MemberRelations
		{
			get { return GetAsQueryable<MemberRelation>(); }
		}

		#endregion


		
	}
}
