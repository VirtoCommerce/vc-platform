angular.module('virtoCommerce.storeModule')
.factory('virtoCommerce.storeModule.stores', ['$resource', function ($resource) {
    return $resource('api/stores', {}, {
        search: { method: 'POST', url: 'api/stores/search' },
        get: { url: 'api/stores/:id' },
        update: { method: 'PUT' },
        queryLoginOnBehalfStores: { url: 'api/stores/allowed/:userId', isArray: true }
    });
}]);