angular.module('platformWebApp')
.factory('platformWebApp.exportImport.resource', ['$resource', function ($resource) {

    return $resource(null, {},
        {
            getNewExportManifest: { url: 'api/platform/export/manifest/new' },
            runExport: { method: 'POST', url: 'api/platform/export' },

            loadExportManifest: { url: 'api/platform/export/manifest/load' },
            runImport: { method: 'POST', url: 'api/platform/import' },
            sampleDataDiscover: { url: 'api/platform/sampledata/discover', isArray: true },
            importSampleData: { method: 'POST', url: 'api/platform/sampledata/import', params: { url: '@url' } },

            taskCancel: { method: 'POST', url: 'api/platform/exortimport/tasks/:jobId/cancel'}
        });
}]);
