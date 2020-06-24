---
title: Extending and using Logging system
description: Extending and using Logging system
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 8
---
## Introduction

VirtoCommerce logging system is based on Semantic Logging Application Block (SLAB). If you have to add logging to your code, you should be familiar with SLAB basics. Read more about SLABВ <a href="http://msdn.microsoft.com/en-us/library/dn440729(v=pandp.60).aspx" rel="nofollow">here</a>.

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
[EventSource(NameВ =В VCEventSources.Base)]
publicВ classВ PaymentGatewaysEventSourceВ :В EventSource
{
}
```

The class is the placeholder for Event methods. Now add EventCodes class into the PaymentGatewaysEventSource class. The EventCodes class will store unique Event ids constants. The class will be filled once new Event is added to the PaymentGatewaysEventSource.

```
[EventSource(NameВ =В VCEventSources.Base)]
publicВ classВ PaymentGatewaysEventSourceВ :В EventSource
{
  publicВ classВ EventCodes
  {
    publicВ constВ intВ PaymentOperationInfoВ =В 11000;
    publicВ constВ intВ PaymentOperationErrorВ =В 11001;
  }
}
```

Next add Event methods with attributes and input parameters according to the logging logic.

```
[EventSource(NameВ =В VCEventSources.Base)]
publicВ classВ PaymentGatewaysEventSourceВ :В EventSource
{
  privateВ staticВ readonlyВ PaymentGatewaysEventSourceВ _logВ =В newВ PaymentGatewaysEventSource();

  publicВ classВ Keywords
  {
    publicВ constВ EventKeywordsВ DiagnosticВ =В (EventKeywords)VCKeywords.Diagnostic;
  }

  publicВ classВ EventCodes
  {
    publicВ constВ intВ PaymentOperationErrorВ =В 11001;
    publicВ constВ intВ PaymentOperationInfoВ =В 11000;
  }
  
  publicВ staticВ PaymentGatewaysEventSourceВ Log
  {
    get
    {
      returnВ _log;
    }
  }

  [Event(EventCodes.PaymentOperationError,В MessageВ =В "PaymentВ gatewayВ failure:В {3},В task:В {0}",В LevelВ =В EventLevel.Critical,В KeywordsВ =В Keywords.Diagnostic)]
  publicВ voidВ PaymentOperationError(stringВ task,В stringВ orderId,В stringВ orderTN,В stringВ message,В stringВ exception)
  {
    this.WriteEvent(EventCodes.PaymentOperationError,В task,В orderId,В orderTN,В message,В exception);
  }
  [Event(EventCodes.PaymentOperationInfo,В MessageВ =В "{2}В -В PaymentВ gatewayВ success:В {3},В task:В {0}",В LevelВ =В EventLevel.Informational,В KeywordsВ =В Keywords.Diagnostic)]
  publicВ voidВ PaymentOperationInfo(stringВ task,В stringВ orderTN,В stringВ message)
  {
    this.WriteEvent(EventCodes.PaymentOperationInfo,В task,В orderTN,В message);
  }
}
```

Next add PaymentGatewaySlabContext class that is inherited from the BaseSlabContext class.

```
publicВ classВ PaymentGatewaysContext:В BaseSlabContext
{
  publicВ stringВ orderIdВ {В get;В set;В }
  publicВ stringВ orderTNВ {В get;В set;В }
  publicВ stringВ messageВ {В get;В set;В }
  publicВ stringВ paymentStatusВ {В get;В set;В }
  publicВ stringВ billingAddressВ {В get;В set;В }
  publicВ stringВ reservationВ {В get;В set;В }
  publicВ stringВ statusDescВ {В get;В set;В }
  publicВ stringВ taskВ {В get;В set;В }
}
```

Note that the context property names match to the Event methods input parameter names. That is important in order to use EventSource "Write" extension. It uses reflection to match input parameters to the context property values.

## Using PaymentGatewayEventSource

There are three ways of using the EventSource

* Use directly calling the Event method
* Collecting logging information into context and calling EventSource вЂњWriteвЂќ extension
* Using SlabInvoker

### Calling Event method

The PaymentGatewayEventSource can be used directly by calling its Event methods:

```
PaymentGatewayEventSource.Log.PaymentOperationInfo(task, orderTN, message);
```

In this case passing task, orderTN and message as input parameters.

### Using Write extension

Another way is to use вЂњWriteвЂќ extension. First create PaymentGatewayContext, collect data that needs to be logged into context properties and execute Write method with EventCode and context as input parameters:

```
PaymentGatewayEventSource.Log.Write(EventCode.PaymentOperationInfo, context);
```

### Using SlabInvoker

SlabInvoker can be used while logging some long running task or portion of tasks. In that case wrap the code of the long running task into SlabInvoker Execute delegate method passing the PaymentGatewayContext as the T parameter of the Invoker. So the SlabInvoker will fix the start time of the task, end time of the task and other basic properties. Also you will be able to add OnFinished, OnError, OnSuccess after the long running task will complete.

```
SlabInvoker<PaymentGatewaysEventSource.PaymentGatewaysContext>.Execute(slab =>
{
  slab.paymentStatus = PaymentStatus.Failed.ToString();
В 
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
В 
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
