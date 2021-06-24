## Extending grid columns

Get 'platformWebApp.ui-grid.extension' in the module run function, then register extension function to add custom column permanently (data-independent) into the list.

*`order2.js`*
```JS
...
gridOptionExtension.registerExtension("customerOrder-list-grid", function (gridOptions) {
    var customColumnDefs = [
        { name: 'newField', displayName: 'orders.blades.customerOrder-list.labels.newField', width: '***' }
    ];

    gridOptions.columnDefs = _.union(gridOptions.columnDefs, customColumnDefs);
});
...
```
The column was added by such manner will be always available.