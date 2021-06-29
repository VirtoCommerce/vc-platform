---
title: Payment Gateway
description: Payment Gateway
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 5
---
## Introduction

Payment gateway is part of order process used for transferring the actual payment to outside provider that is responsible for completing the payment transaction based on given context. Payment gateway is set for each payment method and is configuration based on payment gateway that it uses.

## Interface

To implement new payment gateway you need to implement the interface VirtoCommerce.Foundation.Orders.Services.IPaymentGateway:

```
using System.Collections.Generic;
using System.ServiceModel;
using VirtoCommerce.Foundation.Orders.Model;
namespace VirtoCommerce.Foundation.Orders.Services
{
  [ServiceContract]
  public interface IPaymentGateway
  {
    /// <summary>
    /// Returns the configuration data associated with a gateway.
    /// Sets the configuration gateway data. This data typically includes
    /// information like gateway URL, account info and so on.
    /// </summary>
    /// <value>The settings.</value>
    /// <returns></returns>
    IDictionary<string, string> Settings { get; set; }
    /// <summary>
    /// Processes the payment. Can be used for both positive and negative transactions.
    /// </summary>
    /// <param name="payment">The payment.</param>
    /// <param name="message">The message.</param>
    /// <returns></returns>
    [OperationContract]
    bool ProcessPayment(Payment payment, ref string message);
  }
}
```

The interface has one method called ProcessPayment that returns true if payment was successful or false otherwise. The message parameter is used to set any error or information that can be useful to caller. The Payment is an object that saved to database. It is wise to save payment before calling ProcessPayment and after it completes. The Settings is a dictionary that can contain any information needed to setup and complete payment. In general it should be filled from payment method properties each time an instance if IPaymentGateway implementation is created. Below is the sample code of creating settings:

```
private Dictionary<string, string> CreateSettings(PaymentMethod method)
{
  var settings =  method.PaymentMethodPropertyValues.ToDictionary(property => property.Name, property => property.ToString());
  settings["Gateway"] = method.PaymentGateway.GatewayId;
  return settings;
}
```

## Data structure

The PaymentGateway model is quite simple. It has these fields:

* GatewayId - can be any unique string like GUID or code that identifies gateway.
* ClassType - the actual implementation of the gateway. Has to be full type name ex.: "VirtoCommerce.PaymentGateways.ICharge.IchargePaymentGateway, VirtoCommerce.PaymentGateways"
* Name - Display name of the gateway that is shown in UI,
* SupportsRecurring - identifies if gateway implementation support recurring,
* SupportedTransactionTypes - any combination of enum flags converted to int. Ex.: (int)(TransactionType.Sale | TransactionType.Credit | TransactionType.Void)

Payment gateway stored in database usually contains configuration properties that defines what setting can or must be set for Payment method. The properties can be required or option, string or bool, and also can be choices from dictionary values. Below is a sample code code for create new payment gateway:

```
var icGateway = new PaymentGateway
{
  GatewayId = "gwAuthorizeNet",
  ClassType = "VirtoCommerce.PaymentGateways.ICharge.IchargePaymentGateway, VirtoCommerce.PaymentGateways",
  Name = "Authorize.Net",
  SupportsRecurring = false,
  SupportedTransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)
};
icGateway.GatewayProperties.Add(new GatewayProperty
{
  DisplayName = "Merchant's Gateway login",
  Name = "MerchantLogin",
  ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
  IsRequired = true,
});
icGateway.GatewayProperties.Add(new GatewayProperty
{
  DisplayName = "Merchant's Gateway password",
  Name = "MerchantPassword",
  ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
  IsRequired = true,
});
icGateway.GatewayProperties.Add(new GatewayProperty
{
  DisplayName = "Default URL for a specific Gateway.",
  Name = "GatewayURL",
  ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
  IsRequired = false
});
```

## How it works

After creating payment gateway and saving to database you can go to management tool Settings/Payments and create or edit payment method. You should see you new payment gateway in dropdown:

<img src="../../../assets/images/docs/ccpg.png" />

Then opening Parameters tab you can configure all properties for payment method that are based on selected gateway

<img src="../../../assets/images/docs/ccpp.png" />

When you have correctly configured you payment method the custom implementation of IPaymentGateway should be used. Below is sample code taken from OrderService CreatePayment:

```
public CreatePaymentResult CreatePayment(Payment payment)
{
  var result = new CreatePaymentResult();
  if (string.IsNullOrEmpty(payment.TransactionType))
  {
    result.Message = "Transaction type is required";
    return result;
  }
  var paymentMethod = _paymentMethodRepository.PaymentMethods
    .Expand("PaymentGateway")
    .Expand("PaymentMethodPropertyValues")
    .ExpandAll()
    .FirstOrDefault(p => p.Name.Equals(payment.PaymentMethodId));
  if (paymentMethod == null)
  {
    result.Message = String.Format("Specified payment method \"{0}\" has not been defined.", payment.PaymentMethodId);
    return result;
  }
  var transactionType = (TransactionType)Enum.Parse(typeof(TransactionType), payment.TransactionType);
  if ((((TransactionType)paymentMethod.PaymentGateway.SupportedTransactionTypes) & transactionType) != transactionType)
  {
    result.Message = String.Format("Transaction type {0} is not supported by payment gateway", payment.TransactionType);
    return result;
  }
  _logger.Debug(String.Format("Getting the type \"{0}\".", paymentMethod.PaymentGateway.ClassType));
  var type = Type.GetType(paymentMethod.PaymentGateway.ClassType);
  if (type == null)
  {
    result.Message = String.Format("Specified payment method class \"{0}\" can not be created.", paymentMethod.PaymentGateway.ClassType);
    return result;
  }
  var provider = (IPaymentGateway)Activator.CreateInstance(type);
  provider.Settings = CreateSettings(paymentMethod);
  payment.Status = PaymentStatus.Pending.ToString();
  var message = "";
  _logger.Debug(String.Format("Processing the payment."));
  try
  {
    //Save changes before process
    var orderForm = _orderRepository.Orders.SelectMany(o => o.OrderForms).Expand("OrderGroup/OrderAddresses").First(of => of.OrderFormId == payment.OrderFormId);
    payment.OrderForm = null;
    orderForm.Payments.Add(payment);
    _orderRepository.UnitOfWork.Commit();
    payment.OrderForm = orderForm;
    result.IsSuccess = provider.ProcessPayment(payment, ref message);
    result.Message = message;
    _logger.Debug(String.Format("Payment processed."));
  }
  catch (Exception ex)
  {
    result.Message = ex.Message;
    payment.Status = PaymentStatus.Failed.ToString();
    _logger.Error(ex.Message, ex);
  }
  finally
  {
    var cardPayment = payment as CreditCardPayment;
    if (cardPayment != null)
    {
      cardPayment.CreditCardNumber = String.Empty;
      // Always remove pin
      cardPayment.CreditCardSecurityCode = String.Empty;
    }
    _orderRepository.Update(payment);
    _orderRepository.UnitOfWork.Commit();
  }
  return result;
}
```

