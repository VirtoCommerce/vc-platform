angular.module('platformWebApp')
.factory('platformWebApp.licensingApi', ['$resource', function ($resource) {
    return $resource('api/platform/licensing/getLicense');
}]);