angular.module('catalogModule.resources.properties', [])
.factory('properties', ['$resource', function ($resource) {

	return $resource('api/properties/:id', { id: '@id' }, {
		newProperty: { method: 'GET', url: 'api/properties/getnewproperty'},
		get: { method: 'GET', url: 'api/properties/get/:id' },
        update: { method: 'POST', url: 'api/properties/post/:id' },
        query: { url: 'api/properties/getpropertyvalues', isArray: true },
        delete: { method: 'DELETE', url: 'api/properties/delete/:id' }
    });

}]);