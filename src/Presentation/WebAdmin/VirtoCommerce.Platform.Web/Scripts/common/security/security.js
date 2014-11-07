angular.module('platformWebApp.security', [
    'platformWebApp.security.auth',
    'platformWebApp.security.login'
]).config(['$stateProvider', '$httpProvider', function ($stateProvider, $httpProvider) {

	$httpProvider.interceptors.push('authInterceptor');

	$stateProvider
		.state('loginDialog', {
			url: '/login',
			templateUrl: 'Scripts/common/security/login/login.tpl.html',
			controller: ['$scope', 'authService', function ($scope, authService) {
				$scope.user = {};
				$scope.authError = null;
				$scope.authReason = false;

				$scope.ok = function () {
					// Clear any previous security errors
					$scope.authError = null;
					// Try to login
					authService.login($scope.user.email, $scope.user.password, $scope.user.remember).then(function (loggedIn) {
						if (!loggedIn) {
							$scope.authError = 'invalidCredentials';
						}
					}, function (x) {
						if (angular.isDefined(x.status)) {
							if (x.status == 401) {
								$scope.authError = 'The login or password is incorrect.';
							} else {
								$scope.authError = 'Authentication error (code: ' + x.status + ').';
							}
						} else {
							$scope.authError = 'Authentication error ' + x;
						}
					});
				};

			}]
		});
}]);