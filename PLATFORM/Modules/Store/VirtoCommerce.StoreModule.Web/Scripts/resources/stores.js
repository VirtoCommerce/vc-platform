angular.module('virtoCommerce.storeModule')
.factory('virtoCommerce.storeModule.stores', ['$resource', function ($resource) {
    return $resource('api/stores', {}, {
        get: { url: 'api/stores/:id' },
        update: { method: 'PUT' },
        queryFilterProperties: { url: 'api/stores/:id/filterproperties', isArray: true },
        saveFilterProperties: { url: 'api/stores/:id/filterproperties', method: 'PUT' }
    });
}]);