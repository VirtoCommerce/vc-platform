angular.module('virtoCommerce.storeModule')
.factory('virtoCommerce.storeModule.import', ['$resource', function ($resource) {

	return $resource('api/store/import/:id', { id: '@id' }, {
		run: { method: 'POST', url: 'api/store/import', isArray: false }
	});

}]);

