angular.module('virtoCommerce.orderModule.resources')
.factory('order_res_fulfilmentCenters', ['$resource', function ($resource) {
	return $resource('api/fulfillment/centers');
}]);