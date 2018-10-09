angular.module('platformWebApp')
.factory('platformWebApp.externalSignInService', ['$http', function ($http) {
    return {
        getProviders: function() {
            return $http.get('externalsignin/providers');
        }
    }
}]);
