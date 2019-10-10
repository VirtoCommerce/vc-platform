angular.module('platformWebApp')
.factory('platformWebApp.authService', ['$http', '$rootScope', '$cookieStore', '$state', '$interpolate', '$q', 'platformWebApp.authDataStorage', function ($http, $rootScope, $cookieStore, $state, $interpolate, $q, authDataStorage) {
    var serviceBase = 'api/platform/security/';
    var authContext = {
        userId: null,
        userLogin: null,
        fullName: null,
        permissions: null,
        isAuthenticated: false
    };

    authContext.fillAuthData = function () {
        return $http.get(serviceBase + 'currentuser').then(
            function (results) {
                changeAuth(results.data);
            });
    };

    authContext.login = function (email, password, remember) {       
        var requestData = 'grant_type=password&scope=offline_access&username=' + encodeURIComponent(email) + '&password=' + encodeURIComponent(password);
        return $http.post('connect/token', requestData, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).then(
            function (response) {
                var authData = {
                    token: response.data.access_token,
                    userName: email,
                    expiresAt: getCurrentDateWithOffset(response.data.expires_in),
                    refreshToken: response.data.refresh_token
                };
                authDataStorage.storeAuthData(authData);
                return authContext.fillAuthData().then(function () {
                    return response.data;
                });
            }, function (error) {
                authContext.logout();
                return $q.reject(error);
            }
        );
    };

    authContext.refreshToken = function () {
        var authData = authDataStorage.getStoredData();
        if (authData) {
            var data = 'grant_type=refresh_token&refresh_token=' + encodeURIComponent(authData.refreshToken);
            // NOTE: this method is called by the HTTP interceptor if the access token in the local storage expired.
            //       So we clean the storage before sending the HTTP request - otherwise the HTTP interceptor will
            //       detect expired token and will call this method again, causing the infinite loop.
            authDataStorage.clearStoredData();
            return $http.post('connect/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).then(
                function (response) {
                    var newAuthData = {
                        token: response.data.access_token,
                        userName: response.data.userName,
                        expiresAt: getCurrentDateWithOffset(response.data.expires_in),
                        refreshToken: response.data.refresh_token
                    };
                    authDataStorage.storeAuthData(newAuthData);
                    return newAuthData;
                }, function (err) {
                    authContext.logout();
                    return $q.reject(err);
                });
        } else {
            return $q.reject();
        }
    };

    function getCurrentDateWithOffset(offsetInSeconds) {
        return Date.now() + offsetInSeconds * 1000;
    }

    authContext.requestpasswordreset = function (data) {
        return $http.post(serviceBase + 'users/' + data.userName + '/requestpasswordreset/').then(
            function (results) {
                return results.data;
            });
    };

    authContext.validatepasswordresettoken = function (data) {
        return $http.post(serviceBase + 'users/' + data.userId + '/validatepasswordresettoken', { token: data.code }).then(
            function (results) {
                return results.data;
            });
    };

    authContext.resetpassword = function (data) {
        return $http.post(serviceBase + 'users/' + data.userId + '/resetpasswordconfirm', { token: data.code, newPassword: data.newPassword }).then(
            function (results) {
                return results.data;
            });
    };

    authContext.logout = function () {
        authDataStorage.clearStoredData();
        $http.get(serviceBase + 'logout');
        changeAuth({});
    };

    authContext.checkPermission = function (permission, securityScopes) {
        //first check admin permission
        // var hasPermission = $.inArray('admin', authContext.permissions) > -1;
        var hasPermission = authContext.isAdministrator;
        if (!hasPermission && permission) {
            permission = permission.trim();
            //first check global permissions
            hasPermission = $.inArray(permission, authContext.permissions) > -1;
            if (!hasPermission && securityScopes) {
                if (typeof securityScopes === 'string' || angular.isArray(securityScopes)) {
                    securityScopes = angular.isArray(securityScopes) ? securityScopes : securityScopes.split(',');
                    //Check permissions in scope
                    hasPermission = _.some(securityScopes, function (x) {
                        var permissionWithScope = permission + ":" + x;
                        var retVal = $.inArray(permissionWithScope, authContext.permissions) > -1;
                        //console.log(permissionWithScope + "=" + retVal);
                        return retVal;
                    });
                }
            }
        }
        return hasPermission;
    };

    function changeAuth(user) {
        angular.extend(authContext, user);
        authContext.userLogin = user.userName;
        authContext.fullName = user.userLogin;
        authContext.isAuthenticated = user.userName != null;

        //Interpolate permissions to replace some template to real value
        if (authContext.permissions) {
            authContext.permissions = _.map(authContext.permissions, function (x) {
                return $interpolate(x)(authContext);
            });
        }
        $rootScope.$broadcast('loginStatusChanged', authContext);
    }
    return authContext;
}]);
