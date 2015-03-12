angular.module('virtoCommerce.customerModule.resources', [])
.factory('members', ['$resource', function ($resource) {
    return $resource('api/members', {}, {
        search: {}
    });
}])
.factory('organizations', ['$resource', function ($resource) {
    return $resource('api/organizations/:_id', { _id: '@_id' }, {
        update: { method: 'PUT' }
    });
}])
.factory('contacts', ['$resource', function ($resource) {
    return $resource('api/contacts/:_id', { _id: '@_id' }, {
        update: { method: 'PUT' }
    });
}]);