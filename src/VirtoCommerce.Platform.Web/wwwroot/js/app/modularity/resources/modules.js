angular.module('platformWebApp')
.factory('platformWebApp.modulesApi', ['$resource', function ($resource) {

    return $resource('api/platform/modules', null, {
        getDependencies: { method: 'POST', url: 'api/platform/modules/getmissingdependencies', isArray: true },
        getDependents: { method: 'POST', url: 'api/platform/modules/getdependents', isArray: true },
        install: { method: 'POST', url: 'api/platform/modules/install' },
        update: { method: 'POST', url: 'api/platform/modules/update' },
        uninstall: { method: 'POST', url: 'api/platform/modules/uninstall' },
        installModules: { method: 'POST', url: 'api/platform/modules/install/v2' },
        updateModules: { method: 'POST', url: 'api/platform/modules/update/v2' },
        uninstallModules: { method: 'POST', url: 'api/platform/modules/uninstall/v2' },
        restart: { method: 'POST', url: 'api/platform/modules/restart' },
        autoInstall: { method: 'POST', url: 'api/platform/modules/autoinstall' },
        reload: { method: 'POST', url: 'api/platform/modules/reload', isArray: true },
        validateVersion: { method: 'GET', url: 'api/platform/modules/:id/versions/:version/validate' },
        installVersion: { method: 'POST', url: 'api/platform/modules/:id/versions/:version/install' },
    });
}]);
