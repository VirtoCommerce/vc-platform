﻿angular.module('virtoCommerce.orderModule.resources')
.factory('order_res_stores', ['$resource', function ($resource) {
	return $resource('api/stores');
}]);