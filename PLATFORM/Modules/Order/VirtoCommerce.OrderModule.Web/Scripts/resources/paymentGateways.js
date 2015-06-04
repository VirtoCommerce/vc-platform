angular.module('virtoCommerce.orderModule')
.factory('virtoCommerce.orderModule.order_res_paymentGateways', ['$resource', function ($resource) {
	return $resource('api/paymentgateways');
}]);