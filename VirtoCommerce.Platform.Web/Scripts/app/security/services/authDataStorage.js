angular.module('platformWebApp')
.factory('platformWebApp.authDataStorage', ['localStorageService', function(localStorageService) {
    var service = {
        storeAuthData: function(token, userName, refreshToken) {
            localStorageService.set('authenticationData',
                {
                    token: token,
                    userName: userName,
                    refreshToken: refreshToken
                });
        },
        getStoredData: function() {
            return localStorageService.get('authenticationData');
        },
        clearStoredData: function() {
            localStorageService.remove('authenticationData');
        }
    };

    return service;
}]);
