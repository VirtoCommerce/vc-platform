angular.module('platformWebApp')
	.config(['$stateProvider', '$httpProvider', function ($stateProvider, $httpProvider) {

	    $stateProvider.state('loginDialog', {
	        url: '/login',
	        templateUrl: 'Scripts/common/security/login/login.tpl.html',
	        controller: ['$scope', 'authService', function ($scope, authService) {
	            $scope.user = {};
	            $scope.authError = null;
	            $scope.authReason = false;
	            $scope.loginProgress = false;
	            $scope.ok = function () {
	                // Clear any previous security errors
	                $scope.authError = null;
	                $scope.loginProgress = true;
	                // Try to login
	                authService.login($scope.user.email, $scope.user.password, $scope.user.remember).then(function (loggedIn) {
	                    $scope.loginProgress = false;
	                    if (!loggedIn) {
	                        $scope.authError = 'invalidCredentials';
	                    }
	                }, function (x) {
	                    $scope.loginProgress = false;
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
	    })

	    .state('workspace.securityModule', {
	        url: '/security',
	        template: '<va-blade-container />',
	        controller: [
				'$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
				    var blade = {
				        id: 'security',
				        title: 'Security',
				        subtitle: 'User management',
				        controller: 'securityMainController',
				        template: 'Scripts/common/security/blades/security-main.tpl.html',
				        isClosingDisabled: true
				    };
				    bladeNavigationService.showBlade(blade);
				}
	        ]
	    });
	}])
    .run(['$rootScope', 'mainMenuService', 'widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
        //Register module in main menu
        var menuItem = {
            path: 'browse/security',
            icon: 'fa fa-lock',
            title: 'Security',
            priority: 190,
            action: function () { $state.go('workspace.securityModule'); },
            permission: 'platform:security:query'
        };
        mainMenuService.addMenuItem(menuItem);

        //Register widgets
        widgetService.registerWidget({
            controller: 'accountRolesWidgetController',
            template: 'Scripts/common/security/widgets/accountRolesWidget.tpl.html',
        }, 'accountDetail');
        widgetService.registerWidget({
            controller: 'accountApiWidgetController',
            template: 'Scripts/common/security/widgets/accountApiWidget.tpl.html',
        }, 'accountDetail');
    }]);