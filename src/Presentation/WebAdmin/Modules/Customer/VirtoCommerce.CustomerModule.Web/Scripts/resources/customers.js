angular.module('virtoCommerce.customerModule.resources', [])
.factory('customers', ['$resource', function ($resource) {
    return $resource('api/contacts', {}, {
        search: {},
        get: { url: 'api/contacts/:id', params: { id: '@id' } },
        update: { method: 'PUT' }
    });
}]);