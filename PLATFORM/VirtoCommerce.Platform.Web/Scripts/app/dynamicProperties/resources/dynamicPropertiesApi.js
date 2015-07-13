angular.module('platformWebApp')
.factory('platformWebApp.dynamicProperties.api', ['$resource', function ($resource) {
    return $resource('api/platform/dynamic/types/:id/properties', {}, {
        queryTypes: { url: 'api/platform/dynamic/types', isArray: true },
        delete: { method: 'DELETE', url: 'api/platform/dynamic/types/:id/properties/:propertyId' }
        // get: { url: 'api/platform/dynamic/types/:id/properties' },
        //runExport: { method: 'POST', url: 'api/platform/export' }
    });
}]);