using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.CartModule.Data.Repositories
{
    public class CartRepositoryImpl : EFRepositoryBase, ICartRepository
    {
        public CartRepositoryImpl()
        {
        }

        public CartRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, null, interceptors)
        {
            Configuration.LazyLoadingEnabled = false;
        }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            #region ShoppingCart
            modelBuilder.Entity<ShoppingCartEntity>().HasKey(x => x.Id)
                    .Property(x => x.Id);

            modelBuilder.Entity<ShoppingCartEntity>().ToTable("Cart");
            #endregion

            #region LineItem
            modelBuilder.Entity<LineItemEntity>().HasKey(x => x.Id)
                    .Property(x => x.Id);

            modelBuilder.Entity<LineItemEntity>().HasOptional(x => x.ShoppingCart)
                                       .WithMany(x => x.Items)
                                       .HasForeignKey(x => x.ShoppingCartId).WillCascadeOnDelete(true);

            modelBuilder.Entity<LineItemEntity>().ToTable("CartLineItem");
            #endregion

            #region Shipment
            modelBuilder.Entity<ShipmentEntity>().HasKey(x => x.Id)
                .Property(x => x.Id);

            modelBuilder.Entity<ShipmentEntity>().HasRequired(x => x.ShoppingCart)
                                           .WithMany(x => x.Shipments)
                                           .HasForeignKey(x => x.ShoppingCartId).WillCascadeOnDelete(true);


            modelBuilder.Entity<ShipmentEntity>().ToTable("CartShipment");
            #endregion

            #region ShipmentItemEntity
            modelBuilder.Entity<ShipmentItemEntity>().HasKey(x => x.Id)
                .Property(x => x.Id);


            modelBuilder.Entity<ShipmentItemEntity>().HasRequired(x => x.LineItem)
                                       .WithMany()
                                       .HasForeignKey(x => x.LineItemId).WillCascadeOnDelete(true);

            modelBuilder.Entity<ShipmentItemEntity>().HasRequired(x => x.Shipment)
                                       .WithMany(x => x.Items)
                                       .HasForeignKey(x => x.ShipmentId).WillCascadeOnDelete(false);


            modelBuilder.Entity<ShipmentItemEntity>().ToTable("CartShipmentItem");
            #endregion

            #region Address
            modelBuilder.Entity<AddressEntity>().HasKey(x => x.Id)
                .Property(x => x.Id);

            modelBuilder.Entity<AddressEntity>().HasOptional(x => x.ShoppingCart)
                                       .WithMany(x => x.Addresses)
                                       .HasForeignKey(x => x.ShoppingCartId).WillCascadeOnDelete(true);

            modelBuilder.Entity<AddressEntity>().HasOptional(x => x.Shipment)
                                       .WithMany(x => x.Addresses)
                                       .HasForeignKey(x => x.ShipmentId);

            modelBuilder.Entity<AddressEntity>().HasOptional(x => x.Payment)
                                       .WithMany(x => x.Addresses)
                                       .HasForeignKey(x => x.PaymentId);

            modelBuilder.Entity<AddressEntity>().ToTable("CartAddress");
            #endregion

            #region Payment
            modelBuilder.Entity<PaymentEntity>().HasKey(x => x.Id)
                .Property(x => x.Id);

            modelBuilder.Entity<PaymentEntity>().HasOptional(x => x.ShoppingCart)
                                       .WithMany(x => x.Payments)
                                       .HasForeignKey(x => x.ShoppingCartId).WillCascadeOnDelete(true);

            modelBuilder.Entity<PaymentEntity>().ToTable("CartPayment");
            #endregion

            #region TaxDetail
            modelBuilder.Entity<TaxDetailEntity>().HasKey(x => x.Id)
                        .Property(x => x.Id);


            modelBuilder.Entity<TaxDetailEntity>().HasOptional(x => x.ShoppingCart)
                                       .WithMany(x => x.TaxDetails)
                                       .HasForeignKey(x => x.ShoppingCartId).WillCascadeOnDelete(false);

            modelBuilder.Entity<TaxDetailEntity>().HasOptional(x => x.Shipment)
                                       .WithMany(x => x.TaxDetails)
                                       .HasForeignKey(x => x.ShipmentId).WillCascadeOnDelete(false);

            modelBuilder.Entity<TaxDetailEntity>().HasOptional(x => x.LineItem)
                                       .WithMany(x => x.TaxDetails)
                                       .HasForeignKey(x => x.LineItemId).WillCascadeOnDelete(false);


            modelBuilder.Entity<TaxDetailEntity>().ToTable("CartTaxDetail");
            #endregion

            #region Discount
            modelBuilder.Entity<DiscountEntity>().HasKey(x => x.Id)
                        .Property(x => x.Id);


            modelBuilder.Entity<DiscountEntity>().HasOptional(x => x.ShoppingCart)
                                       .WithMany(x => x.Discounts)
                                       .HasForeignKey(x => x.ShoppingCartId).WillCascadeOnDelete(false);

            modelBuilder.Entity<DiscountEntity>().HasOptional(x => x.Shipment)
                                       .WithMany(x => x.Discounts)
                                       .HasForeignKey(x => x.ShipmentId).WillCascadeOnDelete(false);

            modelBuilder.Entity<DiscountEntity>().HasOptional(x => x.LineItem)
                                       .WithMany(x => x.Discounts)
                                       .HasForeignKey(x => x.LineItemId).WillCascadeOnDelete(false);


            modelBuilder.Entity<DiscountEntity>().ToTable("CartDiscount");
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        #region ICartRepository Members

        public IQueryable<ShoppingCartEntity> ShoppingCarts
        {
            get { return GetAsQueryable<ShoppingCartEntity>(); }
        }

        public IQueryable<AddressEntity> Addresses
        {
            get { return GetAsQueryable<AddressEntity>(); }
        }

        public IQueryable<PaymentEntity> Payments
        {
            get { return GetAsQueryable<PaymentEntity>(); }
        }

        public IQueryable<LineItemEntity> LineItems
        {
            get { return GetAsQueryable<LineItemEntity>(); }
        }
        public IQueryable<ShipmentEntity> Shipments
        {
            get { return GetAsQueryable<ShipmentEntity>(); }
        }

        public ShoppingCartEntity GetShoppingCartById(string id)
        {
            var query = ShoppingCarts.Include(x => x.TaxDetails)
                                     .Include(x => x.Discounts)
                                     .Where(x => x.Id == id);
            var addresses = Addresses.Where(x => x.ShoppingCartId == id).ToArray();
            var payments = Payments.Include(x => x.Addresses).Where(x => x.ShoppingCartId == id).ToArray();
            var lineItems = LineItems.Include(x => x.Discounts)
                                     .Include(x => x.TaxDetails)
                                     .Where(x => x.ShoppingCartId == id).ToArray();
            var shipments = Shipments.Include(x => x.TaxDetails)
                                     .Include(x => x.Discounts)
                                     .Include(x => x.Addresses)
                                     .Include(x => x.Items)
                                     .Where(x => x.ShoppingCartId == id).ToArray();
            return query.FirstOrDefault();
        }
        #endregion
    }
}
