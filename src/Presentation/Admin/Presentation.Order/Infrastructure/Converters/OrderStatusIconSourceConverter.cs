using System;
using System.Threading;
using System.Windows.Data;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.ManagementClient.Order.Infrastructure.Converters
{
    public class OrderStatusIconSourceConverter : IValueConverter
    {
        private static readonly ThreadLocal<OrderStatusIconSourceConverter> _instance = new ThreadLocal<OrderStatusIconSourceConverter>(CreateInstance);
        public static OrderStatusIconSourceConverter Current
        {
            get
            {
                return _instance.Value;
            }
        }

        private static OrderStatusIconSourceConverter CreateInstance()
        {
            return new OrderStatusIconSourceConverter();
        }

        /// <summary>
        /// order and shipment statuses converter to icons
        /// </summary>
        /// <param name="value">order or shipment status as string</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">type: {"ShipmentStatus" | "OrderStatus"}. Defaults to "OrderStatus"</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
	        string imageName = null;
            var valueType = "OrderStatus";

            if (parameter is string)
            {
                valueType = parameter.ToString();
            }

            switch (valueType)
            {
	            case "OrderStatus":
		            {
			            var currentStatus = (OrderStatus)value;

			            switch (currentStatus)
			            {
				            case OrderStatus.OnHold:
					            imageName = "Icon_Order_Hold";
					            break;
				            case OrderStatus.PartiallyShipped:
					            imageName = "Icon_Order_Partiallyshipped";
					            break;
				            case OrderStatus.InProgress:
					            imageName = "Icon_Order";
					            break;
				            case OrderStatus.Completed:
					            imageName = "Icon_Order_Completed";
					            break;
				            case OrderStatus.Cancelled:
					            imageName = "Icon_Order_Cancelled";
					            break;
				            case OrderStatus.AwaitingExchange:
					            imageName = "Icon_Order_AwaitingExchange";
					            break;
				            case OrderStatus.Pending:
				            default:
					            imageName = "Icon_Order";
					            break;
			            }
		            }
		            break;
	            case "ShipmentStatus":
                {
                    try
                    {
                        var currentStatus = (ShipmentStatus) Enum.Parse(typeof (ShipmentStatus), value.ToString());
                        switch (currentStatus)
                        {
                            case ShipmentStatus.AwaitingInventory:
                                imageName = "Icon_AwaitingInventory";
                                break;
                            case ShipmentStatus.Cancelled:
                                imageName = "Icon_Cancelled";
                                break;
                            case ShipmentStatus.InventoryAssigned:
                                imageName = "Icon_Inventoryassigned";
                                break;
                            case ShipmentStatus.OnHold:
                                imageName = "Icon_Hold";
                                break;
                            case ShipmentStatus.Packing:
                                imageName = "Icon_Packing";
                                break;
                            case ShipmentStatus.Released:
                                imageName = "Icon_Released";
                                break;
                            case ShipmentStatus.Shipped:
                                imageName = "Icon_Completed";
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        imageName = string.Empty;
                    }

                    break;
                }
            }

            return imageName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
