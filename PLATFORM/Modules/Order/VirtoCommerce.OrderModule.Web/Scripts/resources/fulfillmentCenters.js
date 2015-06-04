angular.module('virtoCommerce.orderModule')
.factory('virtoCommerce.orderModule.order_res_fulfilmentCenters', ['$resource', function ($resource) {
	return $resource('api/fulfillment/centers');
}]);