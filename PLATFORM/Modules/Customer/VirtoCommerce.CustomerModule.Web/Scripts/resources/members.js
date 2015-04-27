angular.module('virtoCommerce.customerModule')
.factory('virtoCommerce.customerModule.members', ['$resource', function ($resource) {
    return $resource('api/members', {}, {
        search: {}
    });
}])
.factory('virtoCommerce.customerModule.organizations', ['$resource', function ($resource) {
    return $resource('api/organizations/:_id', { _id: '@_id' }, {
        update: { method: 'PUT' }
    });
}])
.factory('virtoCommerce.customerModule.contacts', ['$resource', function ($resource) {
    return $resource('api/contacts/:_id', { _id: '@_id' }, {
        update: { method: 'PUT' }
    });
}]);