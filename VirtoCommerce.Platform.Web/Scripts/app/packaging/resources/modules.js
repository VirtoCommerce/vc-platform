angular.module('platformWebApp')
.factory('platformWebApp.modules', ['$resource', function ($resource) {

    return $resource('api/platform/modules/:id', { id: '@id' }, {
        getModules: { url: 'api/platform/modules', isArray: true },
        install: { url: 'api/platform/modules/install' },
        update: { url: 'api/platform/modules/:id/update' },
        uninstall: { url: 'api/platform/modules/:id/uninstall' },
        restart: { url: 'api/platform/modules/restart' }
    });
}]);
