angular.module('virtoCommerce.coreModule.common.resources')
.factory('paymentgateways', ['$resource', function ($resource) {
    return $resource('api/paymentgateways', {}, {
        // query: { }
    });
}]);