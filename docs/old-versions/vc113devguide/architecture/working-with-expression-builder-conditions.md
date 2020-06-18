---
title: Working with Expression builder conditions
description: Working with Expression builder conditions
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 7
---
## Introduction

Expression builder can be used in cases where action execution or result is base on some dynamically constructed conditions tree evaluation.

Expression builder implementations are used in promotions, dynamic content, pricelist assignments to setup conditions under which promotion, dynamic content or pricelist applied.

## Conditions basics

All conditions must be serializable so should be marked withВ [Serializable] attribute.

All conditions must be ofВ IExpressionAdaptor interface so to implement

```
Expression<Func<IEvaluationContext,В bool>>В GetExpression();
```

method.

## Creating expression builder condition

The expression builder API usage will be described on the cart total condition of the dynamic content.

As described the condition must be serializable and implement IExpressionAdaptor interface.

```
[Serializable]
public class ConditionCartTotalIs : TypedExpressionElementBase, IExpressionAdaptor
{
  ...
}
```

The condition also based on TypedExpressionElementBase abstract class so much of the common conditions functionality implemented there.

The constructor of the class gets IExpressionViewModel as a parameter. It is the parent ViewModel of the Expression Builder.

```
public ConditionCartTotalIs(IExpressionViewModel expressionViewModel)
  : base("Cart total $ []", expressionViewModel)
{
  WithLabel("Cart total is $ ");
  _cartTotalEl = WithElement(new CartTotalElement(expressionViewModel)) as CartTotalElement;
}
```

As shown below the base class constructor gets the first string parameter to set display name (that is how the condition will be available under condition selection context menu), also it gets the parent ViewModel.

```
protected TypedExpressionElementBase(string displayName, IExpressionViewModel expressionViewModel)
```

So in this case the label of the condition in the context menu will be "Cart total $В []"

<img src="../../../assets/images/image2013-10-7_17_31_6.png" />

In the constructor body the label of the condition value prefix is set:

```
WithLabel("Cart total is $ ");
```

<img src="../../../assets/images/image2013-10-7_17_32_16.png" />

and CartTotalElement used as an value setup element:

```
WithElement(new CartTotalElement(expressionViewModel)) as CartTotalElement;
```
