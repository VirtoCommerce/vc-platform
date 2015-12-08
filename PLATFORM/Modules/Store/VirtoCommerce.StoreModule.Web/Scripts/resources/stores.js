angular.module('virtoCommerce.storeModule')
.factory('virtoCommerce.storeModule.stores', ['$resource', function ($resource) {
    return $resource('api/stores', {}, {
        get: { url: 'api/stores/:id' },
        update: { method: 'PUT' },
        queryFilterProperties: { url: 'api/search/storefilterproperties/:id', isArray: true },
        saveFilterProperties: { url: 'api/search/storefilterproperties/:id', method: 'PUT' }
    });
}]);