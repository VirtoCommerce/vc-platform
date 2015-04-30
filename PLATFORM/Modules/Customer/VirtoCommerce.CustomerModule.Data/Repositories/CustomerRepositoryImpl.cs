using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.CustomerModule.Data.Model;

namespace VirtoCommerce.CustomerModule.Data.Repositories
{
	public class CustomerRepositoryImpl : EFRepositoryBase, ICustomerRepository
	{
		public CustomerRepositoryImpl()
		{
		}

		public CustomerRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{
			//this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;
			Database.SetInitializer<CustomerRepositoryImpl>(null);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			InheritanceMapping(modelBuilder);

			MapEntity<Address>(modelBuilder, toTable: "Address");
			MapEntity<ContactPropertyValue>(modelBuilder, toTable: "ContactPropertyValue");
			MapEntity<Email>(modelBuilder, toTable: "Email");
			MapEntity<Note>(modelBuilder, toTable: "Note");
			MapEntity<Phone>(modelBuilder, toTable: "Phone");
			MapEntity<Organization>(modelBuilder, toTable: "Organization");
			

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

			MapEntity<MemberRelation>(modelBuilder, toTable: "MemberRelation");

			#region Contact mapping

			modelBuilder.Entity<Member>()
				.HasMany(c => c.Emails)
				.WithRequired(e => e.Member);
			modelBuilder.Entity<Member>()
				.HasMany(c => c.Phones)
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
		

			#endregion

			modelBuilder.Entity<MemberRelation>()
				.HasRequired(m => m.Descendant)
				.WithMany(m => m.MemberRelations)
				.WillCascadeOnDelete(false);
		}

		#region IFoundationCustomerRepository Members


		public IQueryable<ContactPropertyValue> ContactPropertyValues
		{
			get { return GetAsQueryable<ContactPropertyValue>(); }
		}

		public IQueryable<Address> Addresses
		{
			get { return GetAsQueryable<Address>(); }
		}

		public IQueryable<Email> Emails
		{
			get { return GetAsQueryable<Email>(); }
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

		public IQueryable<Member> Members
		{
			get { return GetAsQueryable<Member>(); }
		}

		public IQueryable<MemberRelation> MemberRelations
		{
			get { return GetAsQueryable<MemberRelation>(); }
		}

		
		public Contact GetContactById(string id)
		{
			var query = Members.Where(x => x.Id == id)
							   .OfType<Contact>()
							   .Include(x => x.Notes)
							   .Include(x => x.Emails)
							   .Include(x => x.ContactPropertyValues)
							   .Include(x => x.Addresses)
							   .Include(x => x.Phones)
							   .Include(x => x.MemberRelations.Select(y=>y.Ancestor));

			return query.FirstOrDefault();
		}

		public Organization GetOrganizationById(string id)
		{
			var query = Members.Where(x => x.Id == id)
							   .OfType<Organization>()
							   .Include(x => x.Notes)
							   .Include(x => x.Emails)
							   .Include(x => x.Addresses)
							   .Include(x => x.Phones)
							   .Include(x => x.MemberRelations);

			return query.FirstOrDefault();

		}
	
		#endregion
	}

}
