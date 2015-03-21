angular.module('virtoCommerce.coreModule.common')
.factory('paymentgateways', ['$resource', function ($resource) {
    return $resource('api/paymentgateways', {}, {
        // query: { }
    });
}]);