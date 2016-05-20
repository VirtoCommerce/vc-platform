angular.module('platformWebApp')
.factory('platformWebApp.modules', ['$resource', function ($resource) {

    return $resource('api/platform/modules', null, {
        getDependencies: { method: 'POST', url: 'api/platform/modules/getmissingdependencies', isArray: true },
        getDependent: { method: 'POST', url: 'api/platform/modules/dependent', isArray: true },
        install: { method: 'POST', url: 'api/platform/modules/install' },
        uninstall: { method: 'POST', url: 'api/platform/modules/uninstall' },
        restart: { method: 'POST', url: 'api/platform/modules/restart' }
    });
}]);
