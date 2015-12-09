angular.module('virtoCommerce.searchModule')
.factory('virtoCommerce.searchModule.search', ['$resource', function ($resource) {
    return $resource('api/search', {}, {
        rebuild: { url: 'api/search/catalogitem/rebuild' },
        queryFilterProperties: { url: 'api/search/storefilterproperties/:id', isArray: true },
        saveFilterProperties: { url: 'api/search/storefilterproperties/:id', method: 'PUT' }
    });
}]);
