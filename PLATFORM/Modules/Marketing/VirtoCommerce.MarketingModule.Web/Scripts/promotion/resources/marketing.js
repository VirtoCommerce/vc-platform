angular.module('virtoCommerce.marketingModule')
.factory('virtoCommerce.marketingModule.promotions', ['$resource', function ($resource) {
    return $resource('api/marketing/promotions/:id', { id: '@Id' }, {
        search: { url: 'api/marketing/search' },
        getNew: { url: 'api/marketing/promotions/new' },
        update: { method: 'PUT' }
    });
}]);