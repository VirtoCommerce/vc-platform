using System;
using System.Linq;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using OrdersModel = VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.ManagementClient.Order.Model
{
    public class CreateRefundModel : NotifyPropertyChanged
    {
        private decimal _amount;
        private string _refundOption;
        private OrdersModel.Payment _selectedPayment;
        private string _confirmationNumber;

        public OrdersModel.Order Order { get; set; }
        public Action OnAfterSuccessfulSubmit { get; set; }

        public decimal Amount
        {
            get { return _amount; }
            set { _amount = value; OnPropertyChanged(); }
        }

        public string RefundOption
        {
            get { return _refundOption; }
            set { _refundOption = value; OnPropertyChanged(); OnPropertyChanged("DestinationText"); }
        }

        private OrdersModel.Payment[] _Payments;
        public OrdersModel.Payment[] Payments
        {
            get
            {
                if (_Payments == null && Order != null)
                {
                    _Payments =
                        Order.OrderForms[0].Payments.Where(x => x.Status == OrdersModel.PaymentStatus.Completed.ToString()
                                                            && (x.TransactionType == OrdersModel.TransactionType.Capture.ToString()
                                                             || x.TransactionType == OrdersModel.TransactionType.Sale.ToString()))
                                           .OrderByDescending(x => x.Created)
                                           .ToArray();
                }
                return _Payments;
            }
        }

        public OrdersModel.Payment SelectedPayment
        {
            get { return _selectedPayment; }
            set { _selectedPayment = value; OnPropertyChanged(); OnPropertyChanged("DestinationText"); }
        }

        public PaymentModel NewPaymentSource { get; set; }

        public string ConfirmationNumber
        {
            get { return _confirmationNumber; }
            set { _confirmationNumber = value; OnPropertyChanged(); }
        }

        public string DestinationText
        {
            get
            {
                string result = null;
                switch (RefundOption)
                {
                    case "original":
                        var cardInfo = (OrdersModel.CreditCardPayment)SelectedPayment;
                        if (cardInfo != null)
                            result = string.Format("{0} ({1})", cardInfo.CreditCardType, cardInfo.CreditCardNumber);
                        break;
                    case "manual":
                        cardInfo = NewPaymentSource.NewPayment;
                        result = string.Format("{0} ({1})", cardInfo.CreditCardType, cardInfo.CreditCardNumber);
                        break;
                }
                return result;
            }
        }

        private bool _isPaymentSubmitted;
        public bool IsPaymentSubmitted
        {
            get { return _isPaymentSubmitted; }
            set
            {
                if (_isPaymentSubmitted != value)
                {
                    _isPaymentSubmitted = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsNewPaymentSourceAvailable
        {
            get { return Payments.Any(x => x is OrdersModel.CreditCardPayment); }
        }
    }
}
