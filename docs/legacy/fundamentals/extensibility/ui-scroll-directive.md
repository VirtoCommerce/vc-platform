---
# Generic UI scroll directive
---
## Introduction
Generic UI scroll directive helps developers add drop-down lists to the UI and bind them to a custom data source. 

## Using the directive

UI scroll implemented as 'uiScrollDropDown' AngularJS directive and has the following features:
* can be binded to a search resource function allowing pagination and filtering 
* single select drop down list or multiple select
* allows binding to a custom data source
* can define data filter expression
* events support

### Basic usage example
*in .html template*:
```HTML
<form>
...
    <ui-scroll-drop-down 
        ng-model="blade.currentEntity.mainReturnsFulfillmentCenterId"
        data="searchFulfillmentCenters(criteria)"
        placeholder="'stores.blades.store-advanced.placeholders.fulfillment-center'">
    </ui-scroll-drop-down>
...
</form>
```

*in underlying .js file*:
```JS
$scope.searchFulfillmentCenters = function (criteria) {
    return fulfillments.search(criteria);
}
```

## Parameters

|Parameter|Description|
|---------|-----------|
|data|Function that provides data to the dropdown list.
|filter|Function that will be applied to the data after each page fetch.|
|multiple|Controls single select or multiple dropdown select mode.|
|onSelect|Function that is triggered when a value is selected.|
|onRemove|Function that is triggered when the selected value is cleared/removed.|
|pageSize|Controls page size on single scroll (default is 20).|
|placeholder|Sets text that is displayed when the dropdown value is not selected.|
|isRequired|The *required* flag value of underlying ui-select directive (*false* by default).|
|isReadOnly|The *disabled* flag value of underlying ui-select directive (*false* by default).|

## Examples

### Data function 
A data function should either return a resource call result (`$promise`) or an array of predefined objects.
```HTML
<ui-scroll-drop-down 
    ng-model="currentEntity"
    data="getData(criteria)">
</ui-scroll-drop-down>
```
Server call:
```JS
$scope.getData = function (criteria) {
    return resource.search(criteria);
}
```
Or predefined data array:
```JS
$scope.currentEntity = 'ID2';
$scope.getData = function () {
    return [{ id: 'ID1', name: 'name 1' }, { id: 'ID2', name: 'name 2' }, { id: 'ID3', name: 'name 3' }];
}
```
### Multiple
To enable multi select mode:
```HTML
<ui-scroll-drop-down 
    multiple
    ng-model="currentEntity"
    data="getData(criteria)">
</ui-scroll-drop-down>
``` 

Model value must be an array type if you use `multiple`:
```JS
$scope.currentEntity = ['ID2', 'ID3'];
```

### Filter expression
If set this function is called after each new page of data is fetched. Does not work with predefined array data function.
```HTML
<ui-scroll-drop-down 
    ng-model="currentEntity"
    data="serverSearch(criteria)"
    filter="entitiesToHideFnc(items)">
</ui-scroll-drop-down>
``` 
```JS
$scope.entitiesToHideFnc = function (items) {
    return _.filter(items, function (x) {
        console.log(x.name);
        if (x.name === 'name2') return false;
        return true;
    });
}
```

### Events
The directive provides on-select and on-delete events:
```HTML
<ui-scroll-drop-down 
    ng-model="currentEntity"
    data="getData(criteria)"
    on-select="onSelect(item, model)"
    on-remove="onRemove(item, model)">
</ui-scroll-drop-down>
``` 
```JS
$scope.onSelect = function (item, model) {
    console.log(item);
}

$scope.onRemove = function (item, model) {
    console.log(item);
}
```
