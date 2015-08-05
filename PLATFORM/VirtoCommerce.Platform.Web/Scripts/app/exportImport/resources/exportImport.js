angular.module('platformWebApp')
.factory('platformWebApp.exportImport.resource', ['$resource', function ($resource) {

    return $resource(null, null, {
    	getNewExportManifest: { url: 'api/platform/export/manifest/new' },
        runExport: { method: 'POST', url: 'api/platform/export' },

        loadExportManifest: { url: 'api/platform/export/manifest/load' },
        runImport: { method: 'POST', url: 'api/platform/import' },
        importSampleData: { url: 'api/platform/sampledata/import' }
    });
}]);