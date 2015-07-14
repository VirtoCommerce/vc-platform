angular.module('platformWebApp')
.factory('platformWebApp.exportImport.resource', ['$resource', function ($resource) {

    return $resource(null, null, {
        getExportersList: { url: 'api/platform/export/modules', isArray: true },
        runExport: { method: 'POST', url: 'api/platform/export' },

        getImportInfo: { url: 'api/platform/import/info' },
        runImport: { method: 'POST', url: 'api/platform/import' }
    });
}]);