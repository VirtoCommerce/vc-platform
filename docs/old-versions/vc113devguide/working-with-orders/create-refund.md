---
title: Create Refund
description: Create Refund
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 2
---
Refunding can be initiated on order of state **Completed**. Sample code:

```
var itemVM = Container.Resolve<ICreateRefundViewModel>(new ParameterOverride("item", InnerItem), new ParameterOverride("defaultAmount", decimal.Zero));
var confirmation = new ConditionalConfirmation();
confirmation.Title = "Create Refund";
confirmation.Content = itemVM;
CommonOrderWizardDialogInteractionRequest.Raise(confirmation, x =>
{
  if (x.Confirmed)
  {
    ReQueryPayments();
  }
});
```

An instance of ICreateRefundViewModel is resolved. Passing required parameters:

* **item** - Order with OrderForms/Payments properties initialized;
* **defaultAmount** - initial amount to display in UI.

Actual refund payment is submited inside the ICreateRefundViewModel wizard. IOrderService.CreatePayment service method is used for actual payment transaction. Sample code for initiating a CreditCardPayment:

```
var payment = new OrdersModel.CreditCardPayment();
switch (InnerModel.RefundOption)
{
  case "original":
    payment.InjectFrom(InnerModel.SelectedPayment);
    break;
  case "manual":
    payment.InjectFrom(InnerModel.NewPaymentSource.NewPayment);
    payment.PaymentMethodId = InnerModel.NewPaymentSource.PaymentMethodName;
    payment.PaymentType = OrdersModel.PaymentType.CreditCard.GetHashCode();

    var initialPayment = InnerModel.Order.OrderForms[0].Payments
      .Where(x => x.PaymentMethodId == payment.PaymentMethodId)
      .OrderByDescending(x => x.Created).First();
    payment.ValidationCode = initialPayment.ValidationCode;
    payment.AuthorizationCode = initialPayment.AuthorizationCode;
    payment.OrderFormId = initialPayment.OrderFormId;
    break;
}
payment.Amount = InnerModel.Amount;
payment.PaymentId = payment.GenerateNewKey();
payment.TransactionType = OrdersModel.TransactionType.Credit.ToString();

var orderService = ServiceLocator.Current.GetInstance<IOrderService>();
try
{
  var paymentResult = orderService.CreatePayment(payment);
}
...
```

A CreatePaymentResult class instance is returned as a CreatePayment() method result. It contains payment creating (processing) details:

* **IsSuccess** - true, if payment was created successfully;
* **Message** - error message or null if payment was created successfully;
* **TransactionId** - transaction confirmation number.

If operation succeeded, payments list should be updated in GUI.
