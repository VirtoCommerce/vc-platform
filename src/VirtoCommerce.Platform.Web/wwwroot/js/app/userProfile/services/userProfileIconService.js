angular.module('platformWebApp')
    .factory('platformWebApp.userProfileIconService', ['$http', '$rootScope', '$interpolate', '$q', '$window', 'platformWebApp.authDataStorage', 'platformWebApp.externalSignInStorage', function ($http, $rootScope, $interpolate, $q, $window, authDataStorage, externalSignInStorage) {

    var service = {
        userIconUrl: null,
        userId: null
    };

    return service;
}]);
