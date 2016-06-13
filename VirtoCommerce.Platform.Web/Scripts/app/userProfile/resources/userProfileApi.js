angular.module('platformWebApp')
.factory('platformWebApp.userProfileApi', ['$resource', function ($resource) {
    return $resource('api/platform/localization/locales'); 
}]);