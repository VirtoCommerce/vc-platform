angular.module('virtoCommerce.packaging.resources.modules', [])
.factory('modules', ['$resource', function ($resource) {

    return $resource('api/modules/:id', { id: '@id' }, {
        getModules: { method: 'GET', url: 'api/modules', isArray: true },
        get: { method: 'GET', url: 'api/modules/:id', isArray: false },
    });
}]);

