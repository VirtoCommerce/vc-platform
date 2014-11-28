angular.module('catalogModule.resources.import', [])
.factory('imports', ['$resource', function ($resource) {

    return $resource('api/import/:id', { id: '@id' }, {
        //remove: { method: 'DELETE', url: 'api/import/delete' },
        //copy: { method: 'GET', url: 'api/import/copy/:id' },
        run: { method: 'POST', url: 'api/import/run' },
        list: { method: 'GET', url: 'api/import/list/:catalogId', isArray: true },
        new: { method: 'GET', url: 'api/import/new/:catalogId' },
        create: { method: 'POST', url: 'api/import/create' },
        update: { method: 'PUT', url: 'api/import/update' },
        updateMappingItems: { method: 'POST', url: 'api/import/updateMappingItems' }
    });

}]);

