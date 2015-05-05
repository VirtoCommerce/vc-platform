angular.module('virtoCommerce.helpdeskModule')
    .factory('zendesk_res_authlink', [
        '$resource', function($resource) {
            return $resource('api/help/authorize', {
                update: { method: 'PUT' }
            });
        }
    ]);