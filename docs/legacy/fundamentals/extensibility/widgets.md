## Introduction and glossary

A **widget** is a relatively simple and intuitive web UI [component](http://en.wikipedia.org/wiki/Software_component) in Virto Commerce platform. It has the role of a transient or auxiliary tile, meaning that:

* it occupies just a portion of a blade;
* provides some useful information displayed in place;
* usually enables opening additional blade with extra information and functionality;
* is reusable and can be added to many blades (**widget containers**) in various places.

A **widget container** is a placeholder control for **widgets**. It enables:

* display widgets in a rectangular area;
* accept options like size, widget column and row count, etc.;
* allow the user to manage widget position within designated area.

A **widget service** is a platform level engine for registering **widgets** and distributing them to appropriate **widget containers**.

## Widget container

Widget container is implemented as **vaWidgetContainer** angularJS directive. We're using [angular-gridster](http://manifestwebdesign.github.io/angular-gridster/) under the hood, therefore our container supports and accepts angular-gridster's options. 

```
<va-widget-container group="itemDetail" blade="blade" gridster-opts="{columns: 6, minRows: 4}"></va-widget-container>
```

|Parameter|Description|
|---------|-----------|
|group|Widget group id. Only widgets from this group will be displayed. Value should resemble the context of the container, e.g. 'itemDetail' and be unique among all widget containers.|
|blade|Reference to parent blade. Passed to each widget inside container.|
|gridster-opts|Angular-gridster options. (optional)|

## Registering a widget

Widget registration is be done by using Platform-level factory. That way **any module can register new widgets to any widget container**: 

1. Reference **'platformWebApp.widgetService'** (as widgetService) in your module's **run** method;
2. Create widget options definition and call 'widgetService.registerWidget'.

```
var variationWidget = {
  controller: 'virtoCommerce.catalogModule.itemVariationWidgetController',
  size: [2, 1],
  isVisible: function (blade) { return !blade.isNew && blade.controller === 'virtoCommerce.catalogModule.itemDetailController'; },
  template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/widgets/itemVariationWidget.tpl.html'
};
widgetService.registerWidget(variationWidget, 'itemDetail');
```

WidgetService.registerWidget parameters:

|Parameter|Description|
|---------|-----------|
|widget|Widget options.|
|containerName|Widget group id. Will be added to widget container having the same group.|

Widget options:

|Member|Description|
|------|-----------|
|controller|AngularJS controller for the widget. Instantiated only on widget rendering, therefore parent blade is accessible in *$scope.blade* variable.|
|size|Widget dimensions: [number of columns, number of rows]. Optional. Default value of [1,1] is applied if not specified.|
|isVisible|Function to control widget visibility. Widget gets visible only if function returns *true*. Optional. Widget is always visible if not specified.|
|template|Template URL for the widget. Check the [Style Guide](../style-guide) for styling guidelines.|

## Widget visibility and permissions

Widget visibility is controlled by defining isVisible method in widget registration options. There are at least 2 use-cases when visibility limiting is needed:

* widget is appropriate only in some scenarios (e.g., hide orders widget while creating new item, hide inventory widget for a digital product);
* access to widget is restricted by security permission. Check sample code below:

```
isVisible: function (blade) { return authService.checkPermission('pricing:query'); },
```
