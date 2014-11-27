angular.module('catalogModule.resources.items', [])
.factory('items', ['$resource', function ($resource) {

    return $resource('api/items/:id', { id: '@id' }, {
        get: { method: 'GET', url: 'api/items/get/:id' },
        remove: { method: 'DELETE', url: 'api/items/delete' },
        linkitems: { method: 'POST', url: 'api/items/linkitems' },
        newItem: { method: 'GET', url: 'api/items/getnewitem' },
        newVariation: { method: 'GET', url: 'api/items/getnewvariation' },
        updateitem: { method: 'POST', url: 'api/items/post' }
    });

}]);

