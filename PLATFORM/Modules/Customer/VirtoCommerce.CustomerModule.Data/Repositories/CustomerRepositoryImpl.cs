using System.Data.Entity;
using System.Linq;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

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
            //Configuration.AutoDetectChangesEnabled = true;
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            #region Member
            modelBuilder.Entity<Member>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<Member>().ToTable("Member");

            #endregion


            #region Contact
            modelBuilder.Entity<Contact>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<Contact>().ToTable("Contact");

            #endregion

            #region Organization
            modelBuilder.Entity<Organization>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<Organization>().ToTable("Organization");
            #endregion

            #region MemberRelation
            modelBuilder.Entity<MemberRelation>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<MemberRelation>().ToTable("MemberRelation");

            modelBuilder.Entity<MemberRelation>().HasRequired(m => m.Descendant)
                                                 .WithMany(m => m.MemberRelations).WillCascadeOnDelete(false);
            #endregion

            #region Address
            modelBuilder.Entity<Address>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<Address>().ToTable("Address");

            modelBuilder.Entity<Address>().HasRequired(m => m.Member)
                                          .WithMany(m => m.Addresses).HasForeignKey(m => m.MemberId)
                                          .WillCascadeOnDelete(true);
            #endregion

            #region Email
            modelBuilder.Entity<Email>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<Email>().ToTable("Email");

            modelBuilder.Entity<Email>().HasRequired(m => m.Member)
                                          .WithMany(m => m.Emails).HasForeignKey(m => m.MemberId)
                                          .WillCascadeOnDelete(true);
            #endregion

            #region Phone
            modelBuilder.Entity<Phone>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<Phone>().ToTable("Phone");

            modelBuilder.Entity<Phone>().HasRequired(m => m.Member)
                                          .WithMany(m => m.Phones).HasForeignKey(m => m.MemberId)
                                          .WillCascadeOnDelete(true);
            #endregion

            #region Note
            modelBuilder.Entity<Note>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<Note>().ToTable("Note");

            modelBuilder.Entity<Note>().HasOptional(m => m.Member)
                                          .WithMany(m => m.Notes).HasForeignKey(m => m.MemberId)
                                          .WillCascadeOnDelete(true);
            #endregion

     

            base.OnModelCreating(modelBuilder);
        }

    
        #region IFoundationCustomerRepository Members


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
                               .Include(x => x.Addresses)
                               .Include(x => x.Phones)
                               .Include(x => x.MemberRelations.Select(y => y.Ancestor));

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
