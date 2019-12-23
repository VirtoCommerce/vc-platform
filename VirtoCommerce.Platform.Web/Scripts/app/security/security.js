angular.module('platformWebApp')
    .config(['$stateProvider', '$httpProvider', function ($stateProvider, $httpProvider) {

        $stateProvider.state('loginDialog',
            {
                url: '/login',
                templateUrl: '$(Platform)/Scripts/app/security/login/login.tpl.html',
                controller: [
                    '$scope', 'platformWebApp.authService', 'platformWebApp.externalSignInService', function ($scope, authService, externalSignInService) {
                        externalSignInService.getProviders().then(
                            function (response) {
                                $scope.externalLoginProviders = response.data;
                            });

                        $scope.user = {};
                        $scope.authError = null;
                        $scope.authReason = false;
                        $scope.loginProgress = false;
                        $scope.ok = function () {
                            // Clear any previous security errors
                            $scope.authError = null;
                            $scope.loginProgress = true;
                            // Try to login
                            authService.login($scope.user.email, $scope.user.password, $scope.user.remember).then(
                                function (loggedIn) {
                                    $scope.loginProgress = false;
                                    if (!loggedIn) {
                                        $scope.authError = 'invalidCredentials';
                                    }
                                },
                                function (x) {
                                    $scope.loginProgress = false;
                                    if (angular.isDefined(x.status)) {
                                        if (x.status == 400 || x.status == 401) {
                                            $scope.authError = 'The login or password is incorrect.';
                                        } else {
                                            $scope.authError = 'Authentication error (code: ' + x.status + ').';
                                        }
                                    } else {
                                        $scope.authError = 'Authentication error ' + x;
                                    }
                                });
                        };

                    }
                ]
            });

        $stateProvider.state('forgotpasswordDialog',
            {
                url: '/forgotpassword',
                templateUrl: '$(Platform)/Scripts/app/security/dialogs/forgotPasswordDialog.tpl.html',
                controller: [
                    '$scope', 'platformWebApp.authService', '$state', function ($scope, authService, $state) {
                        $scope.viewModel = {};
                        $scope.ok = function () {
                            $scope.isLoading = true;
                            $scope.errorMessage = null;
                            authService.requestpasswordreset($scope.viewModel).then(function (retVal) {
                                $scope.isLoading = false;
                                angular.extend($scope, retVal);
                            });
                        };
                        $scope.close = function () {
                            $state.go('loginDialog');
                        };
                    }
                ]
            });

        $stateProvider.state('resetpasswordDialog', {
            url: '/resetpassword/:userId/{code:.*}',
            templateUrl: '$(Platform)/Scripts/app/security/dialogs/resetPasswordDialog.tpl.html',
            controller: ['$rootScope', '$scope', '$stateParams', 'platformWebApp.authService', function ($rootScope, $scope, $stateParams, authService) {
                $scope.viewModel = $stateParams;
                $scope.isValidToken = true;
                $scope.isLoading = true;
                authService.validatepasswordresettoken($scope.viewModel).then(function (retVal) {
                    $scope.isValidToken = retVal;
                    $scope.isLoading = false;
                }, function (response) {
                    $scope.isLoading = false;
                    $scope.errors = response.data.errors;
                });
                $scope.ok = function () {
                    $scope.errorMessage = null;
                    $scope.isLoading = true;
                    authService.resetpassword($scope.viewModel).then(function(retVal) {
                        $scope.isLoading = false;
                        $rootScope.preventLoginDialog = false;
                        angular.extend($scope, retVal);
                    }, function (response) {
                        $scope.viewModel.newPassword = $scope.viewModel.newPassword2 = undefined;
                        $scope.errors = response.data.errors;
                        $scope.isLoading = false;
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

        $stateProvider.state('changePasswordDialog',
            {
                url: '/changepassword',
                templateUrl: '$(Platform)/Scripts/app/security/dialogs/changePasswordDialog.tpl.html',
                params: {
                    onClose: null
                },
                controller: ['$q', '$scope', '$stateParams', 'platformWebApp.accounts', 'platformWebApp.authService', 'platformWebApp.passwordValidationService', function ($q, $scope, $stateParams, accounts, authService, passwordValidationService) {
                    $scope.userName = authService.userName;

                    accounts.get({ id: $stateParams.userName }, function (user) {
                        if (!user || !user.passwordExpired) {
                            $stateParams.onClose();
                        }
                    });

                    $scope.validatePasswordAsync = function(value) {
                        return passwordValidationService.validatePasswordAsync(value);
                    }

                    $scope.postpone = function () {
                        $stateParams.onClose();
                    }

                    $scope.ok = function () {
                        var postData = {
                            NewPassword: $scope.password
                        };
                        accounts.resetCurrentUserPassword(postData, function (data) {
                            $stateParams.onClose();
                        }, function (response) {
                            $scope.errors = response.data.errors;
                        });
                    }
                }]
            });

        $stateProvider.state('changeApiSecretKey',
            {
                url: '/changeapisecretkey',
                templateUrl: '$(Platform)/Scripts/app/security/dialogs/changeApiSecretKeyDialog.tpl.html',
                params: {
                    userName: null,
                    onClose: null
                },
                controller: ['$scope', '$stateParams', 'platformWebApp.accounts', '$state', function ($scope, $stateParams, accounts, $state) {

                    accounts.get({ id: $stateParams.userName }, function (user) {
                        if (user && user.apiAccounts) {
                            var expiredApiAccount = _.find(user.apiAccounts, function (x) { return x.secretKeyExpired; });
                            if (expiredApiAccount) {
                                $scope.user = user;
                                $scope.apiAccount = expiredApiAccount;
                                //Generate new secret key on loading
                                $scope.generate();
                            }
                            else {
                                $stateParams.onClose();
                            }
                        }
                        else {
                            $stateParams.onClose();
                        }
                    });
                    
                    $scope.generate = function () {
                        accounts.generateNewApiKey({}, $scope.apiAccount, function (data) {
                            $scope.apiAccount.secretKey = data.secretKey;
                        });
                    }                  

                    $scope.postpone = function () {
                        $stateParams.onClose();
                    }
                    $scope.save = function () {
                        accounts.update({ }, $scope.user, function() {
                            $stateParams.onClose();
                        }, function(response) {
                            $scope.errors = response.data.errors;
                        });
                    };
                }]
            });
    }])
    .run(['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.metaFormsService', 'platformWebApp.widgetService', '$state', 'platformWebApp.authService', 'platformWebApp.setupWizard',
        function ($rootScope, mainMenuService, metaFormsService, widgetService, $state, authService, setupWizard) {
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
                        name: "userName",
                        templateUrl: "accountUserName.html",
                        priority: 1,
                        isRequired: true
                    },
                    {
                        name: "email",
                        templateUrl: "accountEmail.html",
                        priority: 2
                    },
                    {
                        name: "accountType",
                        templateUrl: "accountTypeSelector.html",
                        priority: 3
                    },
                    {
                        name: "accountInfo",
                        templateUrl: "accountInfo.html",
                        priority: 4
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
            widgetService.registerWidget({
                controller: 'platformWebApp.changeLog.operationsWidgetController',
                template: '$(Platform)/Scripts/app/changeLog/widgets/operations-widget.tpl.html'
            }, 'accountDetail');

            //register setup wizard step - change admin password
            setupWizard.registerStep({
                state: "changePasswordDialog",
                onClose: function () {
                    var step = setupWizard.findStepByState($state.current.name);
                    setupWizard.showStep(step.nextStep);                    
                },
                priority: 20
            });

            //register setup wizard step - change frontend user  API secret key
            setupWizard.registerStep({
                state: "changeApiSecretKey",
                userName: "frontend",
                onClose: function () {
                    var step = setupWizard.findStepByState($state.current.name);
                    setupWizard.showStep(step.nextStep);
                },
                priority: 30
            });
        }]);
