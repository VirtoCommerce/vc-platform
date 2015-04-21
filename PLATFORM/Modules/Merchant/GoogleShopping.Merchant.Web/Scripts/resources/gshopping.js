angular.module('virtoCommerce.gshoppingModule')
    .factory('gshopping_res_items', [
        '$resource', function($resource) {
            return $resource('api/g/products/sync/batch/:id', { id: '@Id' }, {
                query: { isArray: false },
                update: { method: 'PUT' }
            });
        }
    ])

   .factory('gshopping_res_cat_items', [
        '$resource', function ($resource) {
            return $resource('api/g/products/sync/batch/:catalogId/:categoryId', { catalogId: '@Id', categoryId: '@CategoryId' }, {
                update: { method: 'PUT' }
            });
        }
    ]);