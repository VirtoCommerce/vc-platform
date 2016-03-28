angular.module('virtoCommerce.contentModule')
.factory('virtoCommerce.contentModule.menus', ['$resource', function ($resource) {
    return $resource('api/cms/:storeId/menu/', {}, {
        get: { url: 'api/cms/:storeId/menu/', method: 'GET', isArray: true },
        getList: { url: 'api/cms/:storeId/menu/:listId', method: 'GET' },
        checkList: { url: 'api/cms/:storeId/menu/checkname', method: 'GET' },
        update: { url: 'api/cms/:storeId/menu/', method: 'POST' },
        delete: { url: 'api/cms/:storeId/menu/', method: 'DELETE' }
    });
}])
.factory('virtoCommerce.contentModule.pages', ['$resource', function ($resource) {
    return $resource('api/cms/:storeId/pages/', {}, {
        //checkName: { url: 'api/cms/:storeId/pages/checkname', method: 'GET' },
    
        //createBlog: { url: 'api/cms/:storeId/pages/blog/:blogName', method: 'POST' },
        //updateBlog: { url: 'api/cms/:storeId/pages/blog/:blogName/:oldBlogName', method: 'POST' },
    });
}])
.factory('virtoCommerce.contentModule.themes', ['$resource', function ($resource) {
    return $resource(null, null, {
        createTheme: { url: 'api/content/themes/:storeId/createTheme', method: 'POST' },
        cloneTheme: { url: 'api/content/themes/:storeId/cloneTheme', method: 'POST' }
    });
}])
.factory('virtoCommerce.contentModule.contentApi', ['$resource', function ($resource) {
    return $resource('api/content/:contentType/:storeId', null, {
        createFolder: { url: 'api/content/themes/:storeId/folder', method: 'POST' },
        move: { url: 'api/content/themes/:storeId/move' }
    });
}])
.factory('virtoCommerce.contentModule.stores', ['$resource', function ($resource) {
    return $resource('api/stores', {}, {
        get: { url: 'api/stores/:id' },
        update: { method: 'PUT' }
    });
}]);