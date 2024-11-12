angular.module('platformWebApp')
.factory('platformWebApp.modulesApi', ['$resource', function ($resource) {

    return $resource('api/platform/modules', null, {
        getDependencies: { method: 'POST', url: 'api/platform/modules/getmissingdependencies', isArray: true },
        getDependents: { method: 'POST', url: 'api/platform/modules/getdependents', isArray: true },
        install: { method: 'POST', url: 'api/platform/modules/install' },
        update: { method: 'POST', url: 'api/platform/modules/update' },
        uninstall: { method: 'POST', url: 'api/platform/modules/uninstall' },
        restart: { method: 'POST', url: 'api/platform/modules/restart' },
        autoInstall: { method: 'POST', url: 'api/platform/modules/autoinstall' },
        reload: { method: 'POST', url: 'api/platform/modules/reload', isArray: true },
    });
}]);
