angular.module('virtoCommerce.searchModule')
.factory('virtoCommerce.searchModule.search', ['$resource', function ($resource) {
    return $resource('api/search', {}, {
        rebuild: { url: 'api/search/catalogitem/rebuild' }
    });
}]);
