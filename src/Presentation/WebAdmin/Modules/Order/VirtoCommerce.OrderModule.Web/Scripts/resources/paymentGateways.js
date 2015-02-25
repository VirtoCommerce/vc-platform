angular.module('virtoCommerce.orderModule.resources')
.factory('paymentGateways', ['$resource', function ($resource) {
	return $resource('api/paymentgateways');
}]);