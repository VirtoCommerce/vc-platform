angular.module('platformWebApp')
.factory('platformWebApp.authService', ['$http', '$rootScope', '$cookieStore', '$state', '$interpolate', function ($http, $rootScope, $cookieStore, $state, $interpolate) {
	var serviceBase = 'api/platform/security/';
	var authContext = {
		userId : null,
		userLogin: null,
		fullName: null,
		permissions: null,
		isAuthenticated: false
	};

	authContext.fillAuthData = function () {
		$http.get(serviceBase + 'currentuser').then(
			function (results) {
				changeAuth(results.data);
			},
            function (error) { });
	};

	authContext.login = function (email, password, remember) {
		return $http.post(serviceBase + 'login/', { userName: email, password: password, rememberMe: remember }).then(
			function (results) {
				changeAuth(results.data);
				return authContext.isAuthenticated;
			});
	};
	authContext.logout = function () {
		changeAuth({});

		$http.post(serviceBase + 'logout/').then(function (result) {
		});
	};

	authContext.checkPermission = function (permission, securityScopes) {
		//first check admin permission
		// var hasPermission = $.inArray('admin', authContext.permissions) > -1;
		var hasPermission = authContext.isAdministrator;
		if (!hasPermission && permission) {
			permission = permission.trim();
			//first check global permissions
			hasPermission = $.inArray(permission, authContext.permissions) > -1;
			if (!hasPermission && securityScopes)
			{
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
		return hasPermission;
	};

	function changeAuth(results) {
		authContext.userId = results.id;
		authContext.permissions = results.permissions;
		authContext.userLogin = results.userName;
		authContext.fullName = results.userLogin;
		authContext.isAuthenticated = results.userName != null;
		authContext.userType = results.userType;
		authContext.isAdministrator = results.isAdministrator;
		//Interpolate permissions to replace some template to real value
		if (authContext.permissions)
		{
			authContext.permissions = _.map(authContext.permissions, function (x) {
				return $interpolate(x)(authContext);
			});
		}
		$rootScope.$broadcast('loginStatusChanged', authContext);
	}
	return authContext;
}]);
