angular.module('virtoCommerce.coreModule.seo')
.factory('virtoCommerce.coreModule.seoApi', ['$resource', function ($resource) {
    return $resource('api/seoinfos/duplicates');
}]);