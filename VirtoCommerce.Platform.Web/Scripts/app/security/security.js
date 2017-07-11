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

	    $stateProvider.state('forgotpasswordDialog', {
	        url: '/forgotpassword',
	        templateUrl: '$(Platform)/Scripts/app/security/login/forgotPassword.tpl.html',
	        controller: ['$scope', 'platformWebApp.authService', function ($scope, authService) {
	            $scope.viewModel = {};
	            $scope.ok = function () {
	                $scope.isLoading = true;
	                $scope.errorMessage = null;
	                authService.requestpasswordreset($scope.viewModel).then(function (retVal) {
	                    $scope.isLoading = false;
	                    angular.extend($scope, retVal);
	                });
	            };
	        }]
	    })

	    $stateProvider.state('resetpasswordDialog', {
	        url: '/resetpassword/:userId/{code:.*}',
	        templateUrl: '$(Platform)/Scripts/app/security/login/resetPassword.tpl.html',
	        controller: ['$rootScope', '$scope', '$stateParams', 'platformWebApp.authService', function ($rootScope, $scope, $stateParams, authService) {
	            $scope.viewModel = $stateParams;
	            $scope.ok = function () {
	                $scope.errorMessage = null;
	                $scope.isLoading = true;
	                authService.resetpassword($scope.viewModel).then(function (retVal) {
	                    $scope.isLoading = false;
	                    $rootScope.preventLoginDialog = false;
	                    angular.extend($scope, retVal);
	                }, function (x) {
	                    $scope.isLoading = false;
	                    $scope.viewModel.newPassword = $scope.viewModel.newPassword2 = undefined;
	                    if (x.status == 400 && x.data && x.data.message) {
	                        $scope.errorMessage = x.data.message;
	                    } else {
	                        $scope.errorMessage = 'Error ' + x;
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
    .run(['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.metaFormsService', 'platformWebApp.widgetService', '$state', 'platformWebApp.authService',
        function ($rootScope, mainMenuService, metaFormsService, widgetService, $state, authService) {
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

        metaFormsService.registerMetaFields("accountDetails",
        [
            {
                name: "isAdministrator",
                title: "platform.blades.account-detail.labels.is-administrator",
                valueType: "Boolean",
                priority: 0
            },
            {
                name: "accountType",
                templateUrl: "accountTypeSelector.html",
                priority: 1
            },
            {
                name: "accountInfo",
                templateUrl: "accountInfo.html",
                priority: 2
            }
        ]);

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