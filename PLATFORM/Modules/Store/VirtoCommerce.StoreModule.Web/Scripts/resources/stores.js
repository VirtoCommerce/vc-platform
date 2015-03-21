angular.module('virtoCommerce.storeModule.resources.stores', [])
.factory('stores', ['$resource', function ($resource) {
    return $resource('api/stores', {}, {
        get: { url: 'api/stores/:id' },
        update: { method: 'PUT' }
    });
}]);