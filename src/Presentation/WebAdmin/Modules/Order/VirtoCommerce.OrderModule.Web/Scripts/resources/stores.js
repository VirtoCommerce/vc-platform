angular.module('virtoCommerce.orderModule.resources')
.factory('order_stores', ['$resource', function ($resource) {
	return $resource('api/stores');
}]);