angular.module('platformWebApp')
    .factory('platformWebApp.diagnostics', ['$resource', function ($resource) {
        return $resource(null, null, {
            getSystemInfo: { url: 'api/platform/diagnostics/systeminfo' }
        });
    }]);
