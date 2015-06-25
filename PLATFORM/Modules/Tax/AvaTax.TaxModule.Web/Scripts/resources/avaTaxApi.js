angular.module('virtoCommerce.avataxModule')
    .factory('virtoCommerce.avataxModule.ping', [
        '$resource', function($resource) {
            return $resource('api/tax/avatax/ping', {
                query: { isArray: false }
            });
        }
    ]);
