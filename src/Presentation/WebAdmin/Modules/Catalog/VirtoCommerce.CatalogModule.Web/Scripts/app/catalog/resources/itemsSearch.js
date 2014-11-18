angular.module('catalogModule.resources.itemsSearch', [])
.factory('itemsSearch', ['$resource', function ($resource) {

    return $resource('api/listentry/', {},
    {
        listitemssearch: { method: 'POST', isArray: false, url: 'api/listentry/listitemssearch' },
        createlinks: { method: 'POST', url: 'api/listentry/createlinks' },
        deletelinks: { method: 'POST', url: 'api/listentry/deletelinks' },
        query: { method: 'POST', isArray: false, url: 'api/itemssearch/itemssearch' }
    });


}])
.factory('virtualCatalogSearch', ['$resource', function ($resource) {

    return $resource('api/itemssearch/virtualcatalogsearch/', {}, {
        'query': { method: 'GET', isArray: false },
        getcatalogmappinginfo: { method: 'GET', url: 'api/itemssearch/getcatalogmappinginfo' }
    });

}]);
