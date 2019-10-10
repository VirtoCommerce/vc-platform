angular.module('platformWebApp')
    .factory('platformWebApp.externalSignInService', ['$http', '$q', function ($http, $q) {
        return {
            getProviders: function () {
                return $http.get('externalsignin/providers');
            }
        }
    }]);
