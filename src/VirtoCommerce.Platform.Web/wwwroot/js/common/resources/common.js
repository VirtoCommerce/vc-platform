angular.module('platformWebApp')
.factory('platformWebApp.common', ['$resource', function ($resource) {
    return $resource(null, { }, {
        getLoginPageUIOptions: { url: 'api/platform/common/ui/loginPageOptions' }
    });
}]);
