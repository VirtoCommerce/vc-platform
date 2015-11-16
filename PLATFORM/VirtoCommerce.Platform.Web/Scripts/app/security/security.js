angular.module('platformWebApp')
	.config(['$stateProvider', '$httpProvider', function ($stateProvider, $httpProvider) {

	    $stateProvider.state('loginDialog', {
	        url: '/login',
	        templateUrl: '$(Platform)/Scripts/app/security/login/login.tpl.html',
	        controller: ['$scope', 'platformWebApp.authService', function ($scope, authService) {
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
	        templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
	        controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
				    var blade = {
				        id: 'security',
				        title: 'platform.blades.security-main.title',
				        subtitle: 'platform.blades.security-main.subtitle',
				        controller: 'platformWebApp.securityMainController',
				        template: '$(Platform)/Scripts/app/security/blades/security-main.tpl.html',
				        isClosingDisabled: true
				    };
				    bladeNavigationService.showBlade(blade);
				}
	        ]
	    });
	}])
    .run(['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state',  'platformWebApp.authService', function ($rootScope, mainMenuService, widgetService, $state, authService) {
        //Register module in main menu
        var menuItem = {
            path: 'configuration/security',
            icon: 'fa fa-key',
            title: 'platform.menu.security',
            priority: 5,
            action: function () { $state.go('workspace.securityModule'); },
            permission: 'platform:security:access'
        };
        mainMenuService.addMenuItem(menuItem);

        //Register widgets
        widgetService.registerWidget({
            controller: 'platformWebApp.accountRolesWidgetController',
            template: '$(Platform)/Scripts/app/security/widgets/accountRolesWidget.tpl.html',
        }, 'accountDetail');
        widgetService.registerWidget({
            controller: 'platformWebApp.accountApiWidgetController',
            template: '$(Platform)/Scripts/app/security/widgets/accountApiWidget.tpl.html',
        }, 'accountDetail');
    }]);