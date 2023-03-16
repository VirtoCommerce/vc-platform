angular.module('platformWebApp')
    .factory('platformWebApp.externalSignInStorage', ['localStorageService', function (localStorageService) {
        return {
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
    }]);
