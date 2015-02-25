angular.module('virtoCommerce.orderModule.resources')
.factory('stores', ['$resource', function ($resource) {
	return $resource('api/stores');
}]);