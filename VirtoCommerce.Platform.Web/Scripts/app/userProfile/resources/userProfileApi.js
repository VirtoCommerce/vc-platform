angular.module('platformWebApp')
.factory('platformWebApp.userProfileApi', ['$resource', function ($resource) {
    return $resource('api/platform/profiles/currentuser', { }, {
        getLocales: { url: 'api/platform/localization/locales', isArray: true }
    }); 
}]);