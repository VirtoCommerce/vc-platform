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
			Database.SetInitializer<OrderRepositoryImpl>(null);
			Configuration.LazyLoadingEnabled = false;
		}

		public OrderRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null,  interceptors)
		{
			Database.SetInitializer<OrderRepositoryImpl>(null);
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

			#region OperationProperty
			modelBuilder.Entity<OperationPropertyEntity>().HasKey(x => x.Id)
					.Property(x => x.Id);

			modelBuilder.Entity<OperationPropertyEntity>().HasRequired(x => x.Operation)
									   .WithMany(x => x.Properties)
									   .HasForeignKey(x => x.OperationId).WillCascadeOnDelete(true);
			modelBuilder.Entity<OperationPropertyEntity>().ToTable("OrderOperationProperty"); 
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
									   

			modelBuilder.Entity<LineItemEntity>().HasOptional(x => x.Shipment)
									   .WithMany(x => x.Items)
									   .HasForeignKey(x => x.ShipmentId).WillCascadeOnDelete(true);
									   

			modelBuilder.Entity<LineItemEntity>().ToTable("OrderLineItem");
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

		public CustomerOrderEntity GetCustomerOrderById(string id, CustomerOrderResponseGroup responseGroup)
		{
			var query = CustomerOrders.Where(x => x.Id == id)
									  .Include(x => x.Discounts)
									  .Include(x => x.Properties);

			if ((responseGroup & CustomerOrderResponseGroup.WithAddresses) == CustomerOrderResponseGroup.WithAddresses)
			{
				query = query.Include(x => x.Addresses);
			}
			if ((responseGroup & CustomerOrderResponseGroup.WithInPayments) == CustomerOrderResponseGroup.WithInPayments)
			{
				query = query.Include(x => x.InPayments.Select(y => y.Addresses))
							 .Include(x => x.InPayments.Select(y => y.Properties));
			}
			if ((responseGroup & CustomerOrderResponseGroup.WithItems) == CustomerOrderResponseGroup.WithItems)
			{
				query = query.Include(x => x.Items.Select(y => y.Discounts));
			}
			if ((responseGroup & CustomerOrderResponseGroup.WithShipments) == CustomerOrderResponseGroup.WithShipments)
			{
				query = query.Include(x => x.Shipments.Select(y => y.Discounts))
							 .Include(x => x.Shipments.Select(y => y.Items))
							 .Include(x => x.Shipments.Select(y => y.Addresses))
							 .Include(x => x.Shipments.Select(y => y.Properties));
			}
			return query.FirstOrDefault();
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
