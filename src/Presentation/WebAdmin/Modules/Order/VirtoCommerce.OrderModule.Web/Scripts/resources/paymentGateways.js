angular.module('virtoCommerce.orderModule.resources')
.factory('order_res_paymentGateways', ['$resource', function ($resource) {
	return $resource('api/paymentgateways');
}]);