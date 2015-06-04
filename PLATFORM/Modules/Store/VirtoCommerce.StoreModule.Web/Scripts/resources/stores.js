angular.module('virtoCommerce.storeModule')
.factory('virtoCommerce.storeModule.stores', ['$resource', function ($resource) {
    return $resource('api/stores', {}, {
        get: { url: 'api/stores/:id' },
        update: { method: 'PUT' }
    });
}]);