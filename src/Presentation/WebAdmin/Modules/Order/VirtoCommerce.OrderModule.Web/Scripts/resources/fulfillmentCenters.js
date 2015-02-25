angular.module('virtoCommerce.orderModule.resources')
.factory('fulfilmentCenters', ['$resource', function ($resource) {
	return $resource('api/fulfillment/centers');
}]);