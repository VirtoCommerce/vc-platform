angular.module('virtoCommerce.coreModule.common.resources')
.factory('fulfillments', ['$resource', function ($resource) {
    return $resource('api/fulfillment/centers', {}, {
        // query: { }
    });
}]);