angular.module('platformWebApp')
    .factory('platformWebApp.externalSignInStorage', ['localStorageService', function(localStorageService) {
        var service = {
            set: function(data) {
                localStorageService.set('externalSignInData', data);
            },
            get: function() {
                return localStorageService.get('externalSignInData');
            },
            remove: function() {
                localStorageService.remove('externalSignInData');
            }
        };
        return service;
    }]);
