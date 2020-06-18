---
title: Create Refund
description: Create Refund
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 2
---
Refunding can be initiated on order of state **Completed**. Sample code:

```
varВ itemVMВ =В Container.Resolve<ICreateRefundViewModel>(newВ ParameterOverride("item",В InnerItem),В newВ ParameterOverride("defaultAmount",В decimal.Zero));
varВ confirmationВ =В newВ ConditionalConfirmation();
confirmation.TitleВ =В "CreateВ Refund";
confirmation.ContentВ =В itemVM;
CommonOrderWizardDialogInteractionRequest.Raise(confirmation,В xВ =>
{
  ifВ (x.Confirmed)
  {
    ReQueryPayments();
  }
});
```

An instance ofВ ICreateRefundViewModelВ is resolved. Passing required parameters:

* **item** - Order with OrderForms/Payments properties initialized;
* **defaultAmount**В - initial amount to display in UI.

Actual refund payment is submited inside theВ ICreateRefundViewModelВ wizard. IOrderService.CreatePaymentВ service method is used for actual payment transaction. Sample code for initiating aВ CreditCardPayment:

```
varВ paymentВ =В newВ OrdersModel.CreditCardPayment();
switchВ (InnerModel.RefundOption)
{
  caseВ "original":
    payment.InjectFrom(InnerModel.SelectedPayment);
    break;
  caseВ "manual":
    payment.InjectFrom(InnerModel.NewPaymentSource.NewPayment);
    payment.PaymentMethodIdВ =В InnerModel.NewPaymentSource.PaymentMethodName;
    payment.PaymentTypeВ =В OrdersModel.PaymentType.CreditCard.GetHashCode();

    varВ initialPaymentВ =В InnerModel.Order.OrderForms[0].Payments
      .Where(xВ =>В x.PaymentMethodIdВ ==В payment.PaymentMethodId)
      .OrderByDescending(xВ =>В x.Created).First();
    payment.ValidationCodeВ =В initialPayment.ValidationCode;
    payment.AuthorizationCodeВ =В initialPayment.AuthorizationCode;
    payment.OrderFormIdВ =В initialPayment.OrderFormId;
    break;
}
payment.AmountВ =В InnerModel.Amount;
payment.PaymentIdВ =В payment.GenerateNewKey();
payment.TransactionTypeВ =В OrdersModel.TransactionType.Credit.ToString();

varВ orderServiceВ =В ServiceLocator.Current.GetInstance<IOrderService>();
try
{
  varВ paymentResultВ =В orderService.CreatePayment(payment);
}
...
```

AВ CreatePaymentResultВ class instance is returned as aВ CreatePayment() method result. It contains payment creating (processing) details:

* **IsSuccess** - true, if payment was created successfully;
* **Message** - error message or null if payment was createdВ successfully;
* **TransactionId** - transaction confirmation number.

If operation succeeded, payments list should be updated in GUI.
