angular.module('virtoCommerce.coreModule.common')
.factory('virtoCommerce.coreModule.common.paymentgateways', ['$resource', function ($resource) {
    return $resource('api/paymentgateways', {}, {
        // query: { }
    });
}]);