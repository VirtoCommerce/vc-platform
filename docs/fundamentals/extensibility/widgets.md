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

## Color markers

To make widgets easier to tell apart at a glance, every widget tile rendered by a **widget container** (the dashboard and all blades) shows a thin colored accent bar along its left edge.

* **Grouped by module** — the color is derived from the widget's **module**, so all widgets that belong to the same module share one color. The module is the controller prefix up to its `*Module` segment, e.g. `virtoCommerce.pricingModule.pricesWidgetController` → `virtoCommerce.pricingModule`. Controllers without a `*Module` segment (e.g. `platformWebApp.*`) fall back to their first namespace segment.
* **Deterministic** — the module key is hashed into a fixed 64-color palette, so a module always gets the same color, across reloads and across every container its widgets appear in. Users build muscle memory (e.g. all pricing widgets are one color, all order widgets another).
* **Perceptually-balanced palette** — colors are generated in the **OKLCH** color space at constant chroma, so every bar has the same perceived vividness (no washed-out or muddy hues, unlike HSL). Hue is spread with a low-discrepancy sequence over the wheel **excluding the status hue zones** (amber ≈35°, green ≈140°) so a module bar never reads as a warning/success color; lightness is varied independently to keep close hues distinguishable.
* **Secondary cue** — the bar is layered on top of the existing number, label and icon; it never replaces them, so the UI stays usable for color‑blind users (the marker is an aid, not the only signal).
* **Automatic** — no module action is required. Any widget registered through `widgetService.registerWidget` is marked; nested `.list-item` elements inside a widget's own content are not.

### Enabling / disabling

The feature is controlled by a single platform setting, **on by default**:

|Setting|Default|Location|
|-------|-------|--------|
|`VirtoCommerce.Platform.UI.WidgetColorMarkers`|`true`|Settings → User Interface|

When turned off, the accent bars are removed everywhere. The flag is loaded into `$rootScope.widgetColorMarkersEnabled` at startup and the container adds the `__widget-markers` class only when it is enabled.
