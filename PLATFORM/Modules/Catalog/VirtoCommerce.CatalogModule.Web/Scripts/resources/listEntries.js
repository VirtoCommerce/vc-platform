angular.module('virtoCommerce.catalogModule')
.factory('virtoCommerce.catalogModule.listEntries', ['$resource', function ($resource) {
    return $resource('api/catalog/listentries', {},
    {
        listitemssearch: { method: 'POST', url: 'api/catalog/listentries' },
        createlinks: { method: 'POST', url: 'api/catalog/listentrylinks' },
        deletelinks: { method: 'POST', url: 'api/catalog/listentrylinks/delete' },
        move: { method: 'POST', url: 'api/catalog/listentries/move' }
    });
}]);

