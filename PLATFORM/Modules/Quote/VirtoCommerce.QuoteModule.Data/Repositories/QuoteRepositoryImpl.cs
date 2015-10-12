using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.QuoteModule.Data.Model;

namespace VirtoCommerce.QuoteModule.Data.Repositories
{
	public class QuoteRepositoryImpl : EFRepositoryBase, IQuoteRepository
	{
		public QuoteRepositoryImpl()
		{
		}
		public QuoteRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{
			Configuration.LazyLoadingEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			#region AddressEntity
			modelBuilder.Entity<AddressEntity>().HasKey(x => x.Id)
				.Property(x => x.Id);
			modelBuilder.Entity<AddressEntity>().ToTable("QuoteAddress");

			modelBuilder.Entity<AddressEntity>().HasRequired(x => x.QuoteRequest)
								   .WithMany(x => x.Addresses)
								   .HasForeignKey(x => x.QuoteRequestId).WillCascadeOnDelete(true);
			#endregion

			#region AttachmentEntity
			modelBuilder.Entity<AttachmentEntity>().HasKey(x => x.Id)
				.Property(x => x.Id);
			modelBuilder.Entity<AttachmentEntity>().ToTable("QuoteAttachment");

			modelBuilder.Entity<AddressEntity>().HasRequired(x => x.QuoteRequest)
								   .WithMany(x => x.Addresses)
								   .HasForeignKey(x => x.QuoteRequestId).WillCascadeOnDelete(true);
			#endregion

			#region QuoteItemEntity
			modelBuilder.Entity<QuoteItemEntity>().HasKey(x => x.Id)
				.Property(x => x.Id);
			modelBuilder.Entity<QuoteItemEntity>().ToTable("QuoteItem");


			modelBuilder.Entity<QuoteItemEntity>().HasRequired(x => x.QuoteRequest)
									   .WithMany(x => x.Items)
									   .HasForeignKey(x => x.QuoteRequestId).WillCascadeOnDelete(true);
			#endregion

			#region TierPriceEntity
			modelBuilder.Entity<TierPriceEntity>().HasKey(x => x.Id)
				.Property(x => x.Id);
			modelBuilder.Entity<TierPriceEntity>().ToTable("QuoteTierPrice");


			modelBuilder.Entity<TierPriceEntity>().HasRequired(x => x.QuoteItem)
									   .WithMany(x => x.ProposalPrices)
									   .HasForeignKey(x => x.QuoteItemId).WillCascadeOnDelete(true);

			#endregion
			#region QuoteRequestEntity
			modelBuilder.Entity<QuoteRequestEntity>().HasKey(x => x.Id)
				.Property(x => x.Id);
			modelBuilder.Entity<QuoteRequestEntity>().ToTable("QuoteRequest");

			#endregion

		}
		#region IQuoteRepository Members
		public QuoteRequestEntity[] GetQuoteRequestByIds(params string[] ids)
		{
			var retVal = QuoteRequests.Include(x => x.Addresses)
									  .Include(x => x.Attachments)
									  .Include(x => x.Items.Select(y => y.ProposalPrices))
									  .Where(x => ids.Contains(x.Id)).ToArray();
			return retVal;

		}
	

		public IQueryable<AddressEntity> Addresses
		{
			get
			{
				return GetAsQueryable<AddressEntity>();
			}
		}

		public IQueryable<AttachmentEntity> Attachments
		{
			get
			{
				return GetAsQueryable<AttachmentEntity>();
			}
		}

		public IQueryable<QuoteItemEntity> QuoteItems
		{
			get
			{
				return GetAsQueryable<QuoteItemEntity>();
			}
		}

		public IQueryable<QuoteRequestEntity> QuoteRequests
		{
			get
			{
				return GetAsQueryable<QuoteRequestEntity>();
			}
		}
		#endregion
	}
}
