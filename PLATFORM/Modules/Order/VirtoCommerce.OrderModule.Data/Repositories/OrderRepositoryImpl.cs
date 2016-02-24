using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.OrderModule.Data.Repositories
{
    public class OrderRepositoryImpl : EFRepositoryBase, IOrderRepository
    {
        public OrderRepositoryImpl()
        {
        }

        public OrderRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, null, interceptors)
        {
            Configuration.LazyLoadingEnabled = false;
        }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            #region Operation
            modelBuilder.Entity<OperationEntity>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<OperationEntity>().ToTable("OrderOperation");
            #endregion



            #region CustomerOrder
            modelBuilder.Entity<CustomerOrderEntity>().HasKey(x => x.Id)
                    .Property(x => x.Id);

            modelBuilder.Entity<CustomerOrderEntity>().ToTable("CustomerOrder");
            #endregion

            #region LineItem
            modelBuilder.Entity<LineItemEntity>().HasKey(x => x.Id)
                    .Property(x => x.Id);


            modelBuilder.Entity<LineItemEntity>().HasOptional(x => x.CustomerOrder)
                                       .WithMany(x => x.Items)
                                       .HasForeignKey(x => x.CustomerOrderId).WillCascadeOnDelete(true);


            modelBuilder.Entity<LineItemEntity>().ToTable("OrderLineItem");
            #endregion

            #region ShipmentItemEntity
            modelBuilder.Entity<ShipmentItemEntity>().HasKey(x => x.Id)
                .Property(x => x.Id);


            modelBuilder.Entity<ShipmentItemEntity>().HasRequired(x => x.LineItem)
                                       .WithMany()
                                       .HasForeignKey(x => x.LineItemId).WillCascadeOnDelete(true);

            modelBuilder.Entity<ShipmentItemEntity>().HasRequired(x => x.Shipment)
                                       .WithMany(x => x.Items)
                                       .HasForeignKey(x => x.ShipmentId).WillCascadeOnDelete(true);

            modelBuilder.Entity<ShipmentItemEntity>().HasOptional(x => x.ShipmentPackage)
                                       .WithMany(x => x.Items)
                                       .HasForeignKey(x => x.ShipmentPackageId).WillCascadeOnDelete(true);


            modelBuilder.Entity<ShipmentItemEntity>().ToTable("OrderShipmentItem");
            #endregion

            #region ShipmentPackageEntity
            modelBuilder.Entity<ShipmentPackageEntity>().HasKey(x => x.Id)
                .Property(x => x.Id);


            modelBuilder.Entity<ShipmentPackageEntity>().HasRequired(x => x.Shipment)
                                       .WithMany(x => x.Packages)
                                       .HasForeignKey(x => x.ShipmentId).WillCascadeOnDelete(true);


            modelBuilder.Entity<ShipmentPackageEntity>().ToTable("OrderShipmentPackage");
            #endregion

            #region Shipment
            modelBuilder.Entity<ShipmentEntity>().HasKey(x => x.Id)
                .Property(x => x.Id);


            modelBuilder.Entity<ShipmentEntity>().HasRequired(x => x.CustomerOrder)
                                           .WithMany(x => x.Shipments)
                                           .HasForeignKey(x => x.CustomerOrderId).WillCascadeOnDelete(true);



            modelBuilder.Entity<ShipmentEntity>().ToTable("OrderShipment");
            #endregion

            #region Address
            modelBuilder.Entity<AddressEntity>().HasKey(x => x.Id)
                .Property(x => x.Id);

            modelBuilder.Entity<AddressEntity>().HasOptional(x => x.CustomerOrder)
                                       .WithMany(x => x.Addresses)
                                       .HasForeignKey(x => x.CustomerOrderId).WillCascadeOnDelete(true);


            modelBuilder.Entity<AddressEntity>().HasOptional(x => x.Shipment)
                                       .WithMany(x => x.Addresses)
                                       .HasForeignKey(x => x.ShipmentId).WillCascadeOnDelete(true);


            modelBuilder.Entity<AddressEntity>().HasOptional(x => x.PaymentIn)
                                       .WithMany(x => x.Addresses)
                                       .HasForeignKey(x => x.PaymentInId).WillCascadeOnDelete(true);


            modelBuilder.Entity<AddressEntity>().ToTable("OrderAddress");
            #endregion

            #region PaymentIn
            modelBuilder.Entity<PaymentInEntity>().HasKey(x => x.Id)
                .Property(x => x.Id);

            modelBuilder.Entity<PaymentInEntity>().HasOptional(x => x.CustomerOrder)
                                       .WithMany(x => x.InPayments)
                                       .HasForeignKey(x => x.CustomerOrderId).WillCascadeOnDelete(true);


            modelBuilder.Entity<PaymentInEntity>().HasOptional(x => x.Shipment)
                                       .WithMany(x => x.InPayments)
                                       .HasForeignKey(x => x.ShipmentId).WillCascadeOnDelete(true);


            modelBuilder.Entity<PaymentInEntity>().ToTable("OrderPaymentIn");
            #endregion

            #region Discount
            modelBuilder.Entity<DiscountEntity>().HasKey(x => x.Id)
                        .Property(x => x.Id);


            modelBuilder.Entity<DiscountEntity>().HasOptional(x => x.CustomerOrder)
                                       .WithMany(x => x.Discounts)
                                       .HasForeignKey(x => x.CustomerOrderId).WillCascadeOnDelete(true);

            modelBuilder.Entity<DiscountEntity>().HasOptional(x => x.Shipment)
                                       .WithMany(x => x.Discounts)
                                       .HasForeignKey(x => x.ShipmentId).WillCascadeOnDelete(true);

            modelBuilder.Entity<DiscountEntity>().HasOptional(x => x.LineItem)
                                       .WithMany(x => x.Discounts)
                                       .HasForeignKey(x => x.LineItemId).WillCascadeOnDelete(true);


            modelBuilder.Entity<DiscountEntity>().ToTable("OrderDiscount");
            #endregion

            #region TaxDetail
            modelBuilder.Entity<TaxDetailEntity>().HasKey(x => x.Id)
                        .Property(x => x.Id);


            modelBuilder.Entity<TaxDetailEntity>().HasOptional(x => x.CustomerOrder)
                                       .WithMany(x => x.TaxDetails)
                                       .HasForeignKey(x => x.CustomerOrderId).WillCascadeOnDelete(true);

            modelBuilder.Entity<TaxDetailEntity>().HasOptional(x => x.Shipment)
                                       .WithMany(x => x.TaxDetails)
                                       .HasForeignKey(x => x.ShipmentId).WillCascadeOnDelete(true);

            modelBuilder.Entity<TaxDetailEntity>().HasOptional(x => x.LineItem)
                                       .WithMany(x => x.TaxDetails)
                                       .HasForeignKey(x => x.LineItemId).WillCascadeOnDelete(true);


            modelBuilder.Entity<TaxDetailEntity>().ToTable("OrderTaxDetail");
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        #region IOrderRepository Members

        public IQueryable<CustomerOrderEntity> CustomerOrders
        {
            get { return GetAsQueryable<CustomerOrderEntity>(); }
        }

        public IQueryable<ShipmentEntity> Shipments
        {
            get { return GetAsQueryable<ShipmentEntity>(); }
        }

        public IQueryable<PaymentInEntity> InPayments
        {
            get { return GetAsQueryable<PaymentInEntity>(); }
        }

        public IQueryable<AddressEntity> Addresses
        {
            get { return GetAsQueryable<AddressEntity>(); }
        }
        public IQueryable<LineItemEntity> LineItems
        {
            get { return GetAsQueryable<LineItemEntity>(); }
        }

        public CustomerOrderEntity GetCustomerOrderById(string id, CustomerOrderResponseGroup responseGroup)
        {
            var query = CustomerOrders.Where(x => x.Id == id)
                                      .Include(x => x.Discounts)
                                      .Include(x => x.TaxDetails);

            if ((responseGroup & CustomerOrderResponseGroup.WithAddresses) == CustomerOrderResponseGroup.WithAddresses)
            {
                var addresses = Addresses.Where(x => x.CustomerOrderId == id).ToArray();
            }
            if ((responseGroup & CustomerOrderResponseGroup.WithInPayments) == CustomerOrderResponseGroup.WithInPayments)
            {
                var inPayments = InPayments.Where(x => x.CustomerOrderId == id).ToArray();
                var paymentsIds = inPayments.Select(x => x.Id).ToArray();
                var paymentAddresses = Addresses.Where(x => paymentsIds.Contains(x.PaymentInId)).ToArray();
            }
            if ((responseGroup & CustomerOrderResponseGroup.WithItems) == CustomerOrderResponseGroup.WithItems)
            {
                var lineItems = LineItems.Include(x => x.TaxDetails)
                                         .Include(x => x.Discounts)
                                         .Where(x => x.CustomerOrderId == id).ToArray();
            }
            if ((responseGroup & CustomerOrderResponseGroup.WithShipments) == CustomerOrderResponseGroup.WithShipments)
            {
                var shipments = Shipments.Include(x => x.TaxDetails)
                                         .Include(x => x.Discounts)
                                         .Include(x => x.Items)
                                         .Include(x => x.Packages.Select(y => y.Items))
                                         .Where(x => x.CustomerOrderId == id).ToArray();
                var shipmentIds = shipments.Select(x => x.Id).ToArray();
                var addresses = Addresses.Where(x => shipmentIds.Contains(x.ShipmentId)).ToArray();
            }
            return query.FirstOrDefault();
        }

        public CustomerOrderEntity GetCustomerOrderByNumber(string orderNumber, CustomerOrderResponseGroup responseGroup)
        {
            var id = CustomerOrders.Where(x => x.Number == orderNumber).Select(x => x.Id).FirstOrDefault();
            if (id != null)
            {
                return GetCustomerOrderById(id, responseGroup);
            }
            return null;
        }

        public ShipmentEntity GetShipmentById(string id, CustomerOrderResponseGroup responseGroup)
        {
            throw new NotImplementedException();
        }

        public PaymentInEntity GetInPaymentById(string id, CustomerOrderResponseGroup responseGroup)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
