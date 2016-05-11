angular.module('platformWebApp')
.factory('platformWebApp.dynamicProperties.api', ['$resource', function ($resource) {
    return $resource('api/platform/dynamic/types/:id/properties/:propertyId', {}, {
        queryTypes: { url: 'api/platform/dynamic/types', isArray: true },
        update: { method: 'PUT' }
    });
}])
.factory('platformWebApp.dynamicProperties.dictionaryItemsApi', ['$resource', function ($resource) {
    return $resource('api/platform/dynamic/types/:id/properties/:propertyId/dictionaryitems');
}]);