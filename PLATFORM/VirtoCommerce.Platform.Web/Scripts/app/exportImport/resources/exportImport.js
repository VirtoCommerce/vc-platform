angular.module('platformWebApp')
.factory('platformWebApp.exportImport.resource', ['$resource', function ($resource) {

    return $resource(null, null, {
        getExportersList: { url: 'api/export/modules', isArray: true },
        runExport: { method: 'POST', url: 'api/export' }
    });
}]);