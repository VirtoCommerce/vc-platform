angular.module('virtoCommerce.marketingModule')
    .factory('promotions', ['$resource', function ($resource) {
        return $resource('api/marketing/promotions/:id', { id: '@Id' }, {
            getNew: { url: 'api/marketing/promotions/new' },
            update: { method: 'PUT' }
        });
    }]);