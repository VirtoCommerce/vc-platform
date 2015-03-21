using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.Foundation.Data;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.CartModule.Data.Repositories
{
	public class CartRepositoryImpl : EFRepositoryBase, ICartRepository
	{
		public CartRepositoryImpl()
		{
			Database.SetInitializer<CartRepositoryImpl>(null);
			Configuration.LazyLoadingEnabled = false;
		}

		public CartRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, null, interceptors)
		{
			Database.SetInitializer<CartRepositoryImpl>(null);
			Configuration.LazyLoadingEnabled = false;
		}



		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			#region ShoppingCart
			modelBuilder.Entity<ShoppingCartEntity>().HasKey(x => x.Id)
					.Property(x => x.Id);

			modelBuilder.Entity<ShoppingCartEntity>().ToTable("cart_ShoppingCart");
			#endregion

			#region LineItem
			modelBuilder.Entity<LineItemEntity>().HasKey(x => x.Id)
					.Property(x => x.Id);

			modelBuilder.Entity<LineItemEntity>().HasOptional(x => x.ShoppingCart)
									   .WithMany(x => x.Items)
									   .HasForeignKey(x => x.ShoppingCartId);

			modelBuilder.Entity<LineItemEntity>().HasOptional(x => x.Shipment)
									   .WithMany(x => x.Items)
									   .HasForeignKey(x => x.ShipmentId);

			modelBuilder.Entity<LineItemEntity>().ToTable("cart_LineItem");
			#endregion

			#region Shipment
			modelBuilder.Entity<ShipmentEntity>().HasKey(x => x.Id)
				.Property(x => x.Id);

			modelBuilder.Entity<ShipmentEntity>().HasRequired(x => x.ShoppingCart)
										   .WithMany(x => x.Shipments)
										   .HasForeignKey(x => x.ShoppingCartId);


			modelBuilder.Entity<ShipmentEntity>().ToTable("cart_Shipment");
			#endregion

			#region Address
			modelBuilder.Entity<AddressEntity>().HasKey(x => x.Id)
				.Property(x => x.Id);

			modelBuilder.Entity<AddressEntity>().HasOptional(x => x.ShoppingCart)
									   .WithMany(x => x.Addresses)
									   .HasForeignKey(x => x.ShoppingCartId);

			modelBuilder.Entity<AddressEntity>().HasOptional(x => x.Shipment)
									   .WithMany(x => x.Addresses)
									   .HasForeignKey(x => x.ShipmentId);

			modelBuilder.Entity<AddressEntity>().HasOptional(x => x.Payment)
									   .WithMany(x => x.Addresses)
									   .HasForeignKey(x => x.PaymentId);

			modelBuilder.Entity<AddressEntity>().ToTable("cart_Address");
			#endregion

			#region Payment
			modelBuilder.Entity<PaymentEntity>().HasKey(x => x.Id)
				.Property(x => x.Id);

			modelBuilder.Entity<PaymentEntity>().HasOptional(x => x.ShoppingCart)
									   .WithMany(x => x.Payments)
									   .HasForeignKey(x => x.ShoppingCartId);

			modelBuilder.Entity<PaymentEntity>().ToTable("cart_Payment");
			#endregion
		}

		#region IOrderRepository Members

		public IQueryable<ShoppingCartEntity> ShoppingCarts
		{
			get { return GetAsQueryable<ShoppingCartEntity>(); }
		}


		public ShoppingCartEntity GetShoppingCartById(string id)
		{
			var query = ShoppingCarts.Where(x => x.Id == id)
									 .Include(x => x.Addresses)
									 .Include(x => x.Payments.Select(y => y.Addresses))
									 .Include(x => x.Items)
									 .Include(x => x.Shipments.Select(y=> y.Addresses));
			return query.FirstOrDefault();
		}
		#endregion
	}
}
