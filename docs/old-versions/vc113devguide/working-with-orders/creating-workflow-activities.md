---
title: Creating workflow activities
description: Creating workflow activities
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 3
---
## Creating and using Order workflows

Creating and using a workflow consists of several steps:

1. Creating an activity
2. Creating a workflow and configuring itвЂ™s settings
3. Registering the workflow
4. Calling the workflow through services

## Creating an activity

1. Create a new project for activities. Steps to be taken are described in вЂњTo create the activity library projectвЂќ paragraph in <a href="http://msdn.microsoft.com/en-us/library/dd489453.aspx" rel="nofollow">http://msdn.microsoft.com/en-us/library/dd489453.aspx</a> tutorial.
2. Add a reference to VirtoCommerce.OrderWorkflowActivities.dll library.
3. Create activity code class. Add a new class, declare it public and derive it from **OrderActivityBase** class from namespace **VirtoCommerce.OrderWorkflow**. The base class has the
  ```
  protectedВ OrderGroupВ CurrentOrderGroupВ {В get;В set;В }
  ```
  property, which should be used for activity actions. All changes are saved to this property and returned back from activity.
4. Implement activity logic by overriding theВ **Execute**В method.
5. Repeat steps 3-4 for every individual activity needed.

## Creating a workflow and configuring itвЂ™s settings

The new activity can be added to an existing workflow or a new workflow can be created.

1. Create a new project for workflows. Steps to be taken are described in вЂњTo create the activity library projectвЂќ paragraph in <a href="http://msdn.microsoft.com/en-us/library/dd489453.aspx" rel="nofollow">http://msdn.microsoft.com/en-us/library/dd489453.aspx</a> tutorial.
2. Create a workflow. Steps to be taken are described in вЂњTo create the workflowвЂќ paragraph in <a href="http://msdn.microsoft.com/en-us/library/gg983473.aspx" rel="nofollow">http://msdn.microsoft.com/en-us/library/gg983473.aspx</a> tutorial.
3. Open workflow designer. Double-clickВ workflow xamlВ file inВ **Solution Explorer**В to display the workflow in the designer, if it is not already displayed.
4. Fill arguments for a workflow. Required argument data is:

|Name|Direction|Argument type|
|----|---------|-------------|
|OrderGroupArgument|In|OrderGroup|
|ResultArgument|In/Out|WorkflowResult

How to: Use the Argument Designer: <a href="http://msdn.microsoft.com/en-us/library/dd489400.aspx" rel="nofollow">http://msdn.microsoft.com/en-us/library/dd489400.aspx</a>

After filling the arguments in the Argument Designer, they should look like this:

<img src="../../../assets/images/docs/tutorial0.png" />

5. Activities to a workflow are added using a Toolbox. Adding your custom activities to the Toolbox: Open the Toolbox window, if it is not already displayed. Next steps to be taken are described in вЂњTo add an activity to the Toolbox from an assemblyвЂќ paragraph in <a href="http://msdn.microsoft.com/en-us/library/dd797579.aspx" rel="nofollow">http://msdn.microsoft.com/en-us/library/dd797579.aspx</a> tutorial.
6. Drag and drop a **Sequence**В activity from theВ **Control Flow** section of theВ **Toolbox**В onto theВ workflow designer pane.
7. Drag your activity from theВ **Toolbox**В and drop it onto theВ **Sequence**В activity.
8. Setting activity properties. Select the activity just added to the **Sequence** and open properties dialog from context menu:

<img src="../../../assets/images/docs/tutorial1.png" />

Set **OrderGroupArgument** and **ResultArgument** values: вЂњOrderGroupArgumentвЂќ and вЂњResultArgumentвЂќ respectively:

<img src="../../../assets/images/docs/tutorial2.png" />

9. Build current project.

## Registering the workflow

### Configuration

1. Add the following section underВ VirtoCommerceВ sectionGroup to web.config or app.config file:
  ```
  <sectionGroup name="VirtoCommerce">
  <sectionВ name="Workflow"В type="VirtoCommerce.Foundation.Frameworks.Workflow.WorkflowConfiguration, VirtoCommerce.Foundation" />
    ...
  </sectionGroup>
  ```
2. Add the following configuration of workflows underВ VirtoCommerce:
  ```
  <VirtoCommerce>
    ...
    <Workflow>
      <activityProviderВ defaultProvider="ResourceActivityProvider">
        <providers>
          <addВ name="ResourceActivityProvider"В type="VirtoCommerce.Foundation.Frameworks.Workflow.Providers.ResourceWorkflowActivityProvider, VirtoCommerce.Foundation"В />
        </providers>
      </activityProvider>
      <availableWorkflows>
        <addВ name="ShoppingCartValidateWorkflow"В type="VirtoCommerce.OrderWorkflow.ShoppingCartValidateWorkflow, VirtoCommerce.OrderWorkflow"/>
        <addВ name="ShoppingCartPrepareWorkflow"В type="VirtoCommerce.OrderWorkflow.ShoppingCartPrepareWorkflow, VirtoCommerce.OrderWorkflow"/>
        <addВ name="ShoppingCartCheckoutWorkflow"В type="VirtoCommerce.OrderWorkflow.ShoppingCartCheckoutWorkflow, VirtoCommerce.OrderWorkflow"/>
        <addВ name="OrderRecalculateWorkflow"В type="VirtoCommerce.OrderWorkflow.OrderRecalculateWorkflow, VirtoCommerce.OrderWorkflow"/>
        <addВ name="CalculateReturnTotalsWorkflow"В type="VirtoCommerce.OrderWorkflow.CalculateReturnTotalsWorkflow, VirtoCommerce.OrderWorkflow"/>
      </availableWorkflows>
    </В WorkflowВ >
  ```
3. Call workflow using the name used in configuration.В В For example to call order workflow useOrderService.ExecuteWorkflow(string workflowName,В OrderGroupВ orderGroup); method.В В (ex.:_orderService.ExecuteWorkflow("CalculateReturnTotalsWorkflow", order); )

### Activity provider

Default activity provider is using reflection to load workflow activity by name. First it initializes by reading avaiableWorkflows config and creates dictionary mapping for workflow name and full type name. Then provider loads type, creates instance and returns it by casting to System.Activities.Activity. If type cannot be loaded provider will throwВ TypeLoadExeption.

### Available workflows

This configuration line defines mapping between full type name and internal name used to call workflows. When calling workflow by name it is important that name in configuration matches called workflow name.

## Calling the workflow through services

**IOrderService.ExecuteWorkflow** method is used for executing workflows remotely. Typical code:

```
varВ orderServiceВ =В ServiceLocator.Current.GetInstance<IOrderService>();
varВ resultВ =В orderService.ExecuteWorkflow(WorkflowName,В InnerItem);
InnerItem.InjectFrom<CloneInjection>(result.OrderGroup);
```

where variables WorkflowName is a workflow name configured in вЂњRegistering the workflowвЂќ step and InnerItem is an OrderGroup.
