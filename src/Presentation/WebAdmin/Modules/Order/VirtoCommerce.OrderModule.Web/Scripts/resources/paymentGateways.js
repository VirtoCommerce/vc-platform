angular.module('virtoCommerce.orderModule')
.factory('order_res_paymentGateways', ['$resource', function ($resource) {
	return $resource('api/paymentgateways');
}]);