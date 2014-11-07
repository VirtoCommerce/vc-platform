angular.module('catalogModule.resources.categories', [])
.factory('categories', ['$resource', function ($resource) {

    return $resource('api/categories/:categoryId', { categoryId: '@Id' }, {
        get: { method: 'GET', url: 'api/categories/get/:categoryId' },
        newCategory: { method: 'GET', url: 'api/categories/getnewcategory' },
        update: { method: 'POST', url: 'api/categories/post' },
        remove: { method: 'POST', url: 'api/categories/delete' },
        linkcategories: { method: 'POST', url: 'api/categories/linkcategories' }
    });

}]);

