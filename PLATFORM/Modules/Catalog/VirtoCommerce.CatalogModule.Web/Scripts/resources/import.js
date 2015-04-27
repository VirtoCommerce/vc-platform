angular.module('virtoCommerce.catalogModule')
.factory('virtoCommerce.catalogModule.imports', ['$resource', function ($resource) {

	return $resource('api/catalog/importjobs/:id', { id: '@id' }, {
		remove: { method: 'DELETE', url: 'api/catalog/importjobs' },
		run: { method: 'GET', url: 'api/catalog/importjobs/:id/run' },
		list: { method: 'GET', url: 'api/catalog/importjobs', isArray: true },
		get: { method: 'GET', url: 'api/catalog/importjobs/:id', isArray: false },
		new: { method: 'GET', url: 'api/catalog/importjobs/getnew' },
		create: { method: 'POST', url: 'api/catalog/importjobs' },
		update: { method: 'POST', url: 'api/catalog/importjobs' },
        getAutoMapping: { method: 'GET', url: 'api/catalog/importjobs/getautomapping', isArray: true }
    });

}]);

