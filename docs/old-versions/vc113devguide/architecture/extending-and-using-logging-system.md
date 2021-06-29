---
title: Extending and using Logging system
description: Extending and using Logging system
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 8
---
## Introduction

VirtoCommerce logging system is based on Semantic Logging Application Block (SLAB). If you have to add logging to your code, you should be familiar with SLAB basics. Read more about SLAB <a href="http://msdn.microsoft.com/en-us/library/dn440729(v=pandp.60).aspx" rel="nofollow">here</a>.

We expanded SLAB by creating SlabInvoker and extension Write method to write into log without concern about passing parameters order in the event method and it's code. Just store everything you need to log in the context and pass the context to the selected Event.

The VirtoCommerce.Slab project has the base classes that should be used in order to log application outputs, events, information, everything needs to be logged.

## Overview

The logging system basics are:

* EventSource
* EventCodes
* Context
* SlabInvoker
* Write (log) extension

**EventSource**

EventSource is the base class that has basic logging methods and needs to be extended with logging methods (Events). Those events should describe what information has to be logged using input parameters. Attributes describes the level of the log, keyword, message format, event code.

**EventCodes**

EventCode is the unique code of the event in the scope of the EventSource. In order to ease EventCode uniqueness validation EventCodes are defined as constants in the EventSource class directly.

**Context**

Context is the placeholder to store data that needs to be logged. The BaseSlabContext is implemented in the VirtoCommerce.Slab project.

**SlabInvoker**

SlabInvoker is the helper class to use with BaseSlabContext or inherited classes. It should be used in the long running tasks to log basic information on the task. That is start time, end time, duration, error if any. The SlabInvoker also has OnSuccess, OnError, OnFinished delegates that can be invoked after the long running task is completed or if error occurred.

**Write extension**

Write(this EventSource, int EventCode, object Context) is an extension method over the EventSource class. It has two input parameters - the EventCode to call the appropriate Event of the particular EventSource and the Context passed to log the event.

To use the Write extension method - fill the information to be logged into the Context pass it with the EventCode into the method. The internal code will parse the context and match the Event method parameters with the Context properties by name (Case-Sensitive). If the Event with the code found in the EventSource and all the required parameters will be filled, the Event will be raised.

## Extending the Semantic Logging

The VirtoCommerce.Slab has base classes to implement logging functionality. Use it to implement your own Contexts and Event sources to extend the base classes to match the logging requirements.

### BaseSlabContext

BaseSlabContext can be used as the base class for any logging Context. It contains StartTime, EndTime, Duration, Exception, Error, HasError properties. Those can be useful in most logging tasks.
In order to extend the context with required properties, create class that will inherit the BaseSlabContext class.

## Developing logs using VirtoCommerce semantic logging system

There are two ways to use VirtoCommerce logging system. Simple and the complex one (structured).

### Simple logging

If you want to just log some event or output you can use existing VirtoCommerce.Slab EventSource.

You can use either existing Events to log anything or add your specific event method.

To add log the fact of task failure you can use existing VirtoCommerceEventSource TaskFailure event. Just add the next line in the place you need to log the fact.

```
VirtoCommerceEventSource.Log.TaskFailure("Task failure message", "TaskName");
```

Just replace "Task failure message" with the message you need to pass, the second parameter is the name of the failed task.

In case you need log event or output and none of the events match your case you need to add new Event to the VirtoCommerceEventSource.

Go to VirtoCommerceEventSource class which is in the VirtoCommerce.Slab project.

Add unique event code

```
public class EventCodes
{
  ...
  public const int Info = 100500;
  ...
}
```

Add new method with the Event attribute.

```
[Event(EventCodes.Info, Message = "Info: {0}", Level = EventLevel.Informational, Keywords = Keywords.Diagnostic)]
public void Info(string infoMessage)
{
  this.WriteEvent(EventCodes.Info, infoMessage);
}
```

Call the created Event in the place you need to log.

```
VirtoCommerceEventSource.Log.Info("Info message");
```

### Structured logging

This section explains how you can use the VirtoCommerce.Slab library and how you implement different event logging scenarios within your applications and services.
In the next example we will add logs into PaymentGateways module.

Once you decided to put anything into log you need to add reference to the VirtoCommerce.Slab project in order to be able to use base classes described above.

<img src="../../../assets/images/worddav8d75ccfd09f60fce690204386dd83883.png" />

Add new class and name it PaymentGatewaysEventSource that is inherited from the EventSource.

Add attribute EventSource to the PaymentGatewaysEventSource class:

```
[EventSource(Name = VCEventSources.Base)]
public class PaymentGatewaysEventSource : EventSource
{
}
```

The class is the placeholder for Event methods. Now add EventCodes class into the PaymentGatewaysEventSource class. The EventCodes class will store unique Event ids constants. The class will be filled once new Event is added to the PaymentGatewaysEventSource.

```
[EventSource(Name = VCEventSources.Base)]
public class PaymentGatewaysEventSource : EventSource
{
  public class EventCodes
  {
    public const int PaymentOperationInfo = 11000;
    public const int PaymentOperationError = 11001;
  }
}
```

Next add Event methods with attributes and input parameters according to the logging logic.

```
[EventSource(Name = VCEventSources.Base)]
public class PaymentGatewaysEventSource : EventSource
{
  private static readonly PaymentGatewaysEventSource _log = new PaymentGatewaysEventSource();

  public class Keywords
  {
    public const EventKeywords Diagnostic = (EventKeywords)VCKeywords.Diagnostic;
  }

  public class EventCodes
  {
    public const int PaymentOperationError = 11001;
    public const int PaymentOperationInfo = 11000;
  }
  
  public static PaymentGatewaysEventSource Log
  {
    get
    {
      return _log;
    }
  }

  [Event(EventCodes.PaymentOperationError, Message = "Payment gateway failure: {3}, task: {0}", Level = EventLevel.Critical, Keywords = Keywords.Diagnostic)]
  public void PaymentOperationError(string task, string orderId, string orderTN, string message, string exception)
  {
    this.WriteEvent(EventCodes.PaymentOperationError, task, orderId, orderTN, message, exception);
  }
  [Event(EventCodes.PaymentOperationInfo, Message = "{2} - Payment gateway success: {3}, task: {0}", Level = EventLevel.Informational, Keywords = Keywords.Diagnostic)]
  public void PaymentOperationInfo(string task, string orderTN, string message)
  {
    this.WriteEvent(EventCodes.PaymentOperationInfo, task, orderTN, message);
  }
}
```

Next add PaymentGatewaySlabContext class that is inherited from the BaseSlabContext class.

```
public class PaymentGatewaysContext: BaseSlabContext
{
  public string orderId { get; set; }
  public string orderTN { get; set; }
  public string message { get; set; }
  public string paymentStatus { get; set; }
  public string billingAddress { get; set; }
  public string reservation { get; set; }
  public string statusDesc { get; set; }
  public string task { get; set; }
}
```

Note that the context property names match to the Event methods input parameter names. That is important in order to use EventSource "Write" extension. It uses reflection to match input parameters to the context property values.

## Using PaymentGatewayEventSource

There are three ways of using the EventSource

* Use directly calling the Event method
* Collecting logging information into context and calling EventSource `Write` extension
* Using SlabInvoker

### Calling Event method

The PaymentGatewayEventSource can be used directly by calling its Event methods:

```
PaymentGatewayEventSource.Log.PaymentOperationInfo(task, orderTN, message);
```

In this case passing task, orderTN and message as input parameters.

### Using Write extension

Another way is to use `Write` extension. First create PaymentGatewayContext, collect data that needs to be logged into context properties and execute Write method with EventCode and context as input parameters:

```
PaymentGatewayEventSource.Log.Write(EventCode.PaymentOperationInfo, context);
```

### Using SlabInvoker

SlabInvoker can be used while logging some long running task or portion of tasks. In that case wrap the code of the long running task into SlabInvoker Execute delegate method passing the PaymentGatewayContext as the T parameter of the Invoker. So the SlabInvoker will fix the start time of the task, end time of the task and other basic properties. Also you will be able to add OnFinished, OnError, OnSuccess after the long running task will complete.

```
SlabInvoker<PaymentGatewaysEventSource.PaymentGatewaysContext>.Execute(slab =>
{
  slab.paymentStatus = PaymentStatus.Failed.ToString();
 
  try
  {
    var response = gateway.Send(request, description);
    if (!response.Approved)
    {
      slab.paymentStatus = payment.Status = PaymentStatus.Denied.ToString();
      slab.message = mes = "Transaction Declined: " + response.Message;
      retVal = false;
      return;
    }
    slab.statusDesc = info.StatusDesc = response.Message;
 
    slab.paymentStatus = payment.Status = PaymentStatus.Completed.ToString();
  }
  catch (Exception ex)
  {
    slab.paymentStatus = payment.Status = PaymentStatus.Failed.ToString();
    throw new ApplicationException(ex.Message);
  }
})
.OnError(PaymentGatewaysEventSource.Log, PaymentGatewaysEventSource.EventCodes.PaymentOperationError)
.OnSuccess(PaymentGatewaysEventSource.Log, PaymentGatewaysEventSource.EventCodes.PaymentOperationInfo);
```
