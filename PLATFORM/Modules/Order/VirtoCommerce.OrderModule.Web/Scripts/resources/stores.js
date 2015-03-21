angular.module('virtoCommerce.orderModule')
.factory('order_res_stores', ['$resource', function ($resource) {
	return $resource('api/stores');
}]);