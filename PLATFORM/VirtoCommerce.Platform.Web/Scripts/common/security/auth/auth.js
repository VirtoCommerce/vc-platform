angular.module('platformWebApp')
.directive('vaPermission', ['platformWebApp.authService', function (authService) {
    return {
        link: function (scope, element, attrs) {
            if (attrs.vaPermission) {
                var permissionValue = attrs.vaPermission.trim();
                var notPermissionFlag = permissionValue[0] === '!';
                if (notPermissionFlag) {
                    permissionValue = permissionValue.slice(1).trim();
                }

                function toggleVisibilityBasedOnPermission() {
                    var hasPermission = authService.checkPermission(permissionValue);

                    if (hasPermission && !notPermissionFlag || !hasPermission && notPermissionFlag)
                        element.show();
                    else
                        element.hide();
                }

                toggleVisibilityBasedOnPermission();
                scope.$on('loginStatusChanged', toggleVisibilityBasedOnPermission);
            }
        }
    };
}])
.factory('platformWebApp.authService', ['$http', '$rootScope', '$cookieStore', '$state', function ($http, $rootScope, $cookieStore, $state) {
    var serviceBase = 'api/security/';
    var authContext = {
        userLogin: null,
        fullName: null,
        permissions: null,
        isAuthenticated: false
    };

    authContext.fillAuthData = function () {
        $http.get(serviceBase + 'usersession').then(
			function (results) {
			    changeAuth(results.data);
			});
    };

    authContext.login = function (email, password, remember) {
        return $http.post(serviceBase + 'login/', { userName: email, password: password, rememberMe: remember }).then(
			function (results) {
			    changeAuth(results.data);
			    return authContext.isAuthenticated;
			});
    };
    authContext.logout = function () {
        $http.post(serviceBase + 'logout/').then(function (result) {

            authContext.isAuthenticated = false;
            authContext.userLogin = null;
            authContext.fullName = null;
            authContext.permissions = null;
            authContext.userType = null;

            $rootScope.$broadcast('loginStatusChanged', authContext);
        });
    };

    authContext.checkPermission = function (permission) {
        //first check admin permission
        // var hasPermission = $.inArray('admin', authContext.permissions) > -1;
        var hasPermission = authContext.userType == 'Administrator';
        if (!hasPermission) {
            permission = permission.trim();
            hasPermission = $.inArray(permission, authContext.permissions) > -1;
        }
        return hasPermission;
    };

    function changeAuth(results) {
        authContext.permissions = results.permissions;
        authContext.userLogin = results.userName;
        authContext.fullName = results.userLogin;
        authContext.isAuthenticated = results.userName != null;
        authContext.userType = results.userType;
        $rootScope.$broadcast('loginStatusChanged', authContext);
    }
    return authContext;
}]);
