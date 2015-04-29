angular.module('virtoCommerce.gshoppingModule')
    .factory('virtoCommerce.gshoppingModule.items', [
        '$resource', function($resource) {
            return $resource('api/g/products/sync/batch/:id', { id: '@Id' }, {
                query: { isArray: false },
                update: { method: 'PUT' }
            });
        }
    ])

   .factory('virtoCommerce.gshoppingModule.catalog_items', [
        '$resource', function ($resource) {
            return $resource('api/g/products/sync/batch/:catalogId/:categoryId', { catalogId: '@Id', categoryId: '@CategoryId' }, {
                update: { method: 'PUT' }
            });
        }
    ]);