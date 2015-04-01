angular.module('virtoCommerce.marketingModule')
    .factory('promotions', ['$resource', function ($resource) {
        return $resource('api/marketing/promotions/:id', { id: '@Id' }, {
            update: { method: 'PUT' }
        });
    }]);