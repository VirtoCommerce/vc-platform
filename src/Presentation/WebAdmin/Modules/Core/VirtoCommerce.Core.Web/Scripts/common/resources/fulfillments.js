angular.module('virtoCommerce.coreModule.common.resources')
.factory('fulfillments', ['$resource', function ($resource) {
    return $resource('api/fulfillment/centers/:_id', { _id: '@_id' }, {
        // query: { }
        update: { method: 'PUT' }
    });
}]);