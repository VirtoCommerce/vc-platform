using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.Services;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Order.Model;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Implementations
{
    public class CreateRefundViewModel : WizardViewModelBase, ICreateRefundViewModel
    {
        public CreateRefundViewModel(
            IViewModelsFactory<IRefundDetailsStepViewModel> detailsVmFactory,
            IViewModelsFactory<IRefundSummaryStepViewModel> summaryVmFactory,
            Foundation.Orders.Model.Order item,
            decimal defaultAmount)
        {

            InnerModel = new CreateRefundModel { Order = item, Amount = defaultAmount };

            var itemParameter = new KeyValuePair<string, object>("item", InnerModel);
            RegisterStep(detailsVmFactory.GetViewModelInstance(itemParameter));
            RegisterStep(summaryVmFactory.GetViewModelInstance(itemParameter));
        }

        protected CreateRefundViewModel(CreateRefundModel item)
        {
            InnerModel = item;
        }

        #region ICreateRefundViewModel Members

        public CreateRefundModel InnerModel { get; private set; }

        public Action OnAfterSuccessfulSubmit
        {
            set { InnerModel.OnAfterSuccessfulSubmit = value; }
        }

        #endregion

        #region IWizardStep Members

        public override bool IsValid
        {
            get { return true; }
        }

        public override bool IsLast
        {
            get
            {
                return this is IRefundSummaryStepViewModel;
            }
        }

        public override string Description
        {
            get { return "Enter payment details".Localize(); }
        }
        #endregion

        protected void InnerModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnIsValidChanged();
        }

        internal static PaymentModel GetNewPaymentModel(Foundation.Orders.Model.Order order, IPaymentMethodRepository repository, StoreClient client)
        {
            var method = repository.PaymentMethods.Where(x => x.Name == "CreditCard").FirstOrDefault();

            var store = client.GetStoreById(order.StoreId);

            var cardTypes = store.CardTypes.Select(x => new ListModel(x.CardType, x.CardType)).ToArray();

            var months = new[]
                {
                    new ListModelInt("01 - January", 1),
                    new ListModelInt("02 - February", 2),
                    new ListModelInt("03 - March", 3),
                    new ListModelInt("04 - April", 4),
                    new ListModelInt("05 - May", 5),
                    new ListModelInt("06 - June", 6),
                    new ListModelInt("07 - July", 7),
                    new ListModelInt("08 - August", 8),
                    new ListModelInt("09 - September", 9),
                    new ListModelInt("10 - October", 10),
                    new ListModelInt("11 - November", 11),
                    new ListModelInt("12 - December", 12)
                };
            var years = new List<ListModelInt>();
            for (var index = DateTime.Now.Year; index <= DateTime.Now.Year + 10; index++)
            {
                years.Add(new ListModelInt(index.ToString(CultureInfo.InvariantCulture), index));
            }

            var result = new PaymentModel
            {
                Months = months,
                Years = years.ToArray(),
                CardTypes = cardTypes
            };

            if (method != null)
            {
                result.NewPayment.PaymentMethodId = method.PaymentMethodId;
                result.NewPayment.PaymentMethodName = method.Name;
            }

            return result;
        }
    }

    public class RefundDetailsStepViewModel : CreateRefundViewModel, IRefundDetailsStepViewModel
    {
        public RefundDetailsStepViewModel(CreateRefundModel item, IPaymentMethodRepository repository, StoreClient client)
            : base(item)
        {
            item.NewPaymentSource = GetNewPaymentModel(item.Order, repository, client);

            InnerModel.PropertyChanged += InnerModel_PropertyChanged;
            InnerModel.NewPaymentSource.NewPayment.PropertyChanged += InnerModel_PropertyChanged;
        }

        public override bool IsValid
        {
            get
            {
                InnerModel.IsPaymentSubmitted = false;

                var retval = InnerModel.Amount > 0 && !string.IsNullOrEmpty(InnerModel.RefundOption);

                if (retval)
                    switch (InnerModel.RefundOption)
                    {
                        case "original":
                            retval = InnerModel.SelectedPayment != null;
                            break;
                        case "manual":
                            retval = InnerModel.NewPaymentSource.Validate();
                            break;
                    }

                return retval;
            }
        }
    }

    public class RefundSummaryStepViewModel : CreateRefundViewModel, IRefundSummaryStepViewModel
    {
        private string _submitPaymentResultMessage;
        private string _transactionId;

        private bool _createPaymentSucceeded;
        private readonly IOrderService _orderService;

        public RefundSummaryStepViewModel(CreateRefundModel item, IOrderService orderService)
            : base(item)
        {
            _orderService = orderService;
            SubmitPaymentCommand = new DelegateCommand(RaiseSubmitPaymentInteractionRequest, () => !_createPaymentSucceeded);
        }

        public DelegateCommand SubmitPaymentCommand { get; set; }

        public string SubmitPaymentResultMessage
        {
            get { return _submitPaymentResultMessage; }
            private set { _submitPaymentResultMessage = value; OnPropertyChanged(); }
        }

        public string TransactionId
        {
            get { return _transactionId; }
            private set { _transactionId = value; OnPropertyChanged(); }
        }

        public override bool IsValid
        {
            get
            {
                return _createPaymentSucceeded;
            }
        }

        public override bool IsBackTrackingDisabled
        {
            get
            {
                return _createPaymentSucceeded;
            }
        }

        private void RaiseSubmitPaymentInteractionRequest()
        {
            var payment = new Foundation.Orders.Model.CreditCardPayment();

            switch (InnerModel.RefundOption)
            {
                case "original":
                    payment.InjectFrom(InnerModel.SelectedPayment);

                    break;
                case "manual":
                    payment = InnerModel.NewPaymentSource.NewPayment;
                    payment.PaymentType = Foundation.Orders.Model.PaymentType.CreditCard.GetHashCode();

                    var initialPayment = InnerModel.Payments
                        .Where(x => x is Foundation.Orders.Model.CreditCardPayment)
                        .OrderByDescending(x => x.Created).FirstOrDefault();
                    if (initialPayment != null)
                    {
                        payment.ValidationCode = initialPayment.ValidationCode;
                        payment.AuthorizationCode = initialPayment.AuthorizationCode;
                        payment.OrderFormId = initialPayment.OrderFormId;
                    }
                    break;
            }
            payment.Amount = InnerModel.Amount;
            payment.PaymentId = payment.GenerateNewKey();
            payment.TransactionType = Foundation.Orders.Model.TransactionType.Credit.ToString();

            try
            {
                var paymentResult = _orderService.CreatePayment(payment);
                _createPaymentSucceeded = paymentResult.IsSuccess;
                if (_createPaymentSucceeded && InnerModel.OnAfterSuccessfulSubmit != null)
                    InnerModel.OnAfterSuccessfulSubmit();

                TransactionId = paymentResult.TransactionId;
                SubmitPaymentResultMessage = paymentResult.Message;

            }
            finally
            {
                InnerModel.IsPaymentSubmitted = true;
                OnPropertyChanged("IsValid");
            }
        }
    }
}
