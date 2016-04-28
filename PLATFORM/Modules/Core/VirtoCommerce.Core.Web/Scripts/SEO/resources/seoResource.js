angular.module('virtoCommerce.coreModule.seo')
.factory('virtoCommerce.coreModule.seoApi', ['$resource', function ($resource) {
    return $resource('api/seoinfos/duplicates', null, {
        batchUpdate: { url: 'api/seoinfos/batchupdate', method: 'PUT' }
    });
}]);