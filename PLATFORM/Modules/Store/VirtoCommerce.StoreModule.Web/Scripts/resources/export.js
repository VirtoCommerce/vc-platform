angular.module('virtoCommerce.storeModule')
.factory('virtoCommerce.storeModule.export', ['$resource', function ($resource) {

	return $resource('api/store/export/:id', { id: '@id' }, {
		run: { method: 'POST', url: 'api/store/export', isArray: false },
	});

}]);

