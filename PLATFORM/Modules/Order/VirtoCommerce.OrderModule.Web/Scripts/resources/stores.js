angular.module('virtoCommerce.orderModule')
.factory('virtoCommerce.orderModule.order_res_stores', ['$resource', function ($resource) {
	return $resource('api/stores');
}]);