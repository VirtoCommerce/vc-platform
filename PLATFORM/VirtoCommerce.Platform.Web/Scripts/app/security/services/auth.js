angular.module('platformWebApp')
.factory('platformWebApp.authService', ['$http', '$rootScope', '$cookieStore', '$state', function ($http, $rootScope, $cookieStore, $state) {
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
		authContext.userId = results.id;
		authContext.permissions = results.permissions;
		authContext.userLogin = results.userName;
		authContext.fullName = results.userLogin;
		authContext.isAuthenticated = results.userName != null;
		authContext.userType = results.userType;
		$rootScope.$broadcast('loginStatusChanged', authContext);
	}
	return authContext;
}]);
