angular.module('virtoCommerce.amazonModule')
    .factory('virtoCommerce.amazonModule.items', [
        '$resource', function($resource) {
            return $resource('api/amazon/products/sync/batch/:id', { id: '@Id' }, {
                query: { isArray: false },
                update: { method: 'PUT' }
            });
        }
    ])

   .factory('virtoCommerce.amazonModule.catalog_items', [
        '$resource', function ($resource) {
            return $resource('api/amazon/products/sync/batch/:catalogId/:categoryId', { catalogId: '@Id', categoryId: '@CategoryId' }, {
                update: { method: 'PUT' }
            });
        }
    ]);