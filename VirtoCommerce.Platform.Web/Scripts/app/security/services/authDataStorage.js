angular.module('platformWebApp')
.factory('platformWebApp.authDataStorage', ['localStorageService', function(localStorageService) {
    var service = {
        storeAuthData: function(token, userName) {
            localStorageService.set('authenticationData',
                {
                    token: token,
                    userName: userName
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
