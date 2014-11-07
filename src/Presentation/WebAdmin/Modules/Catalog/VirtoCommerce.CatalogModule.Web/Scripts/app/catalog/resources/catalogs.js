angular.module('catalogModule.resources.catalogs', [])
.factory('catalogs', ['$resource', function ($resource) {

    return $resource('api/catalogs/:id', { id: '@Id' }, {
        get: { method: 'GET', url: 'api/catalogs/get/:id' },
        getCatalogs: { method: 'GET', url: 'api/catalogs/getcatalogs', isArray: true },
        newCatalog: { method: 'GET', url: 'api/catalogs/getnewcatalog' },
        newVirtualCatalog: { method: 'GET', url: 'api/catalogs/getnewvirtualcatalog' },
        update: { method: 'PUT', url: 'api/catalogs/put/:id' },
        getCatalogLanguages: { method: 'GET', url: 'api/catalogs/getcataloglanguages/:id', isArray: true },
        updateCatalogLanguages: { method: 'POST', url: 'api/catalogs/updatecataloglanguages' },
        delete: { method: 'DELETE', url: 'api/catalogs/delete/:id' }
    });
}]);

