angular.module('virtoCommerce.catalogModule')
.factory('virtoCommerce.catalogModule.import', ['$resource', function ($resource) {

	return $resource('api/catalog/import', {}, {
		getMappingConfiguration: { method: 'GET', url: 'api/catalog/import/mappingconfiguration', isArray: false },
		run: { method: 'POST', url: 'api/catalog/import', isArray: false }
	});

}]);

