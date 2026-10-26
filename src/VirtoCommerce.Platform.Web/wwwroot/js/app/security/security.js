angular.module('platformWebApp')
    .config(['$stateProvider', '$httpProvider', function ($stateProvider, $httpProvider) {
        $stateProvider.state('loginDialog',
            {
                url: '/login',
                templateUrl: '$(Platform)/Scripts/app/security/login/login.tpl.html',
                data: { allowAnonymous: true },
                controller: [
                    '$scope', '$translate', '$log',
                    'platformWebApp.authService', 'platformWebApp.twoFactorSignInState', 'platformWebApp.signInMethodService',
                    function ($scope, $translate, $log, authService, twoFactorSignInState, signInMethodService) {
                        $scope.signInMethods = [];
                        $scope.showPassword = false;
                        $scope.showPlainLogin = true;

                        signInMethodService.getSignInMethods().then(function (signInMethods) {
                            $scope.showPlainLogin = _.some(signInMethods, function (signInMethod) {
                                return signInMethod.kind === 'Password';
                            });

                            $scope.signInMethods = _.where(signInMethods, { canSignIn: true });
                        });

                        $scope.user = {};
                        $scope.authError = null;
                        $scope.authReason = false;
                        $scope.loginProgress = false;

                        $scope.signIn = function (signInMethod) {
                            $scope.authError = null;
                            $scope.loginProgress = true;

                            signInMethodService.signIn(signInMethod).then(
                                function () {
                                    $scope.loginProgress = false;
                                },
                                function (response) {
                                    $scope.loginProgress = false;
                                    $scope.authError = resolveSignInError(response);
                                });
                        };

                        $scope.ok = function () {
                            // Clear any previous security errors
                            $scope.authError = null;
                            $scope.loginProgress = true;
                            // Try to login
                            authService.login($scope.user.email, $scope.user.password, $scope.user.remember).then(
                                function (result) {
                                    $scope.loginProgress = false;
                                    if (result && result.requiresTwoFactor && twoFactorSignInState.exists()) {
                                        twoFactorSignInState.go({ rememberMe: $scope.user.remember });
                                    } else if (!result || !result.succeeded) {
                                        $scope.authError = resolvePasswordSignInError(result);
                                    }
                                },
                                function (response) {
                                    $scope.loginProgress = false;
                                    $scope.authError = resolveSignInError(response);
                                });
                        };

                        function resolvePasswordSignInError(result) {
                            if (result && result.isLockedOut) {
                                return $translate.instant('platform.blades.login.errors.account-locked');
                            }
                            if (result && result.requiresTwoFactor) {
                                return $translate.instant('platform.blades.login.errors.two-factor-unavailable');
                            }
                            return $translate.instant('platform.blades.login.errors.login-failed');
                        }

                        function resolveSignInError(response) {
                            const status = response && response.status;

                            if (status === 400) {
                                const code = response.data && response.data.code;

                                return code === 'user_is_locked_out' || code === 'user_is_temporary_locked_out'
                                    ? $translate.instant('platform.blades.login.errors.account-locked')
                                    : $translate.instant('platform.blades.login.errors.login-failed');
                            }

                            // Do not expose raw exception details to the user — log them instead and show a generic, localized message.
                            $log.error('Login request failed', response);

                            if (status === 401) {
                                return $translate.instant('platform.blades.login.errors.login-failed');
                            }
                            if (status > 0) {
                                return $translate.instant('platform.blades.login.errors.technical-error', { code: status });
                            }
                            return $translate.instant('platform.blades.login.errors.unknown-error');
                        }

                        $scope.togglePassword = function () {
                            $scope.showPassword = !$scope.showPassword;
                        };
                    }
                ]
            });

        $stateProvider.state('forgotpasswordDialog',
            {
                url: '/forgotpassword',
                templateUrl: '$(Platform)/Scripts/app/security/dialogs/forgotPasswordDialog.tpl.html',
                data: { allowAnonymous: true },
                controller: ['$scope', 'platformWebApp.authService', '$state', '$interval', function ($scope, authService, $state, $interval) {
                    $scope.viewModel = {};
                    $scope.ok = function () {
                        $scope.isLoading = true;
                        $scope.errorMessage = null;
                        authService.requestpasswordreset($scope.viewModel).then(function (result) {
                            $scope.isLoading = false;
                            angular.extend($scope, result);

                            if ($scope.nextRequestAt) {
                                $scope.formattedCountdown = getCountdown($scope.nextRequestAt);
                            }
                            else {
                                $scope.succeeded = true;
                            }

                        }, function (response) {
                            $scope.isLoading = false;
                            $scope.errorMessage = response.data.message;
                            if (!$scope.errors) {
                                $scope.errors = [];
                            }
                            $scope.errors.push(response.data.message);
                        });
                    };
                    $scope.close = function () {
                        $state.go('loginDialog');
                    };

                    $interval(function () {
                        $scope.formattedCountdown = getCountdown($scope.nextRequestAt);
                        if (!$scope.formattedCountdown) {
                            $scope.nextRequestAt = undefined;
                        }
                        return;
                    }, 1000);

                    function getCountdown(date) {
                        if (!date) {
                            return undefined;
                        }

                        var time = Math.floor(new Date(date).getTime() - new Date().getTime());

                        if (time <= 0) {
                            return undefined;
                        }

                        var seconds = Math.floor((time / 1000) % 60);
                        var minutes = Math.floor(time / 60000);

                        return minutes.toString().padStart(2, '0') + ':' + seconds.toString().padStart(2, '0');
                    }
                }
                ]
            });

        $stateProvider.state('resetpasswordDialog', {
            url: '/resetpassword/:userId/{code:.*}',
            templateUrl: '$(Platform)/Scripts/app/security/dialogs/resetPasswordDialog.tpl.html',
            data: { allowAnonymous: true },
            controller: ['$scope', '$stateParams', 'platformWebApp.authService', function ($scope, $stateParams, authService) {
                $scope.viewModel = $stateParams;
                $scope.isValidToken = true;
                $scope.isLoading = true;
                authService.validatepasswordresettoken($scope.viewModel).then(function (retVal) {
                    $scope.isValidToken = retVal;
                    $scope.isLoading = false;
                    $scope.viewModel = { userId: $scope.viewModel.userId, code: $scope.viewModel.code, newPassword: '', newPassword2: '' };
                }, function (response) {
                    $scope.isLoading = false;
                    $scope.errors = response.data.errors;
                });
                $scope.ok = function () {
                    $scope.succeeded = undefined;
                    $scope.errors = null;
                    $scope.isLoading = true;
                    authService.resetpassword($scope.viewModel).then(function (retVal) {
                        $scope.isLoading = false;
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
            })

            .state('changePasswordDialog', {
                url: '/changepassword',
                templateUrl: '$(Platform)/Scripts/app/security/dialogs/changePasswordDialog.tpl.html',
                params: {
                    onClose: null
                },
                resolve: {
                    dialog: function () {
                        return {};
                    }
                },
                controller: 'platformWebApp.changePasswordDialog'
            })

            .state('contact-admin', {
                url: '/contact-admin',
                templateUrl: '$(Platform)/Scripts/app/security/dialogs/contact-admin.tpl.html',
                controller: ['$scope', 'platformWebApp.authService', function ($scope, authService) {
                    $scope.logout = authService.logout;
                }]
            });
    }])

    .controller('platformWebApp.changePasswordDialog', ['$rootScope', '$state', '$scope', '$stateParams', 'platformWebApp.accounts', 'platformWebApp.authService', 'platformWebApp.passwordValidationService', 'dialog', function ($rootScope, $state, $scope, $stateParams, accounts, authService, passwordValidationService, dialog) {
        if (!authService.isAuthenticated) {
            $state.go('loginDialog');
        }

        $scope.requireCurrentPassword = dialog.requireCurrentPassword !== false;
        $scope.userName = authService.userName;
        $scope.canCancel = !authService.passwordExpired;

        $scope.validatePasswordAsync = (value) => {
            $scope.validatedPassword = value;
            delete $scope.errors;
            return passwordValidationService.validatePasswordAsync(value);
        };

        $scope.cancel = () => {
            $scope.$dismiss('cancel');
        };

        $scope.ok = () => {
            accounts.changeCurrentUserPassword($scope.postData, (result) => {
                if (result.succeeded) {
                    authService.passwordExpired = false;
                    authService.daysTillPasswordExpiry = -1;
                    $rootScope.$emit('userPasswordChanged', authService);

                    if (angular.isFunction($stateParams.onClose)) {
                        $stateParams.onClose();
                    } else if (angular.isFunction($scope.$close)) {
                        $scope.$close(true);
                    } else {
                        // redirect to home page (after initial platform setup)
                        $state.go('workspace');
                    }
                } else {
                    showErrors(result);
                }
            }, (response) => {
                showErrors(response.data);
            });
        };

        var showErrors = (result) => {
            $scope.postData = {};
            $scope.errors = result.errors;
            $scope.succeeded = false;
        };
    }])

    .run(['$transitions', 'platformWebApp.mainMenuService', 'platformWebApp.metaFormsService', 'platformWebApp.widgetService', '$state', 'platformWebApp.authService',
        '$window', 'platformWebApp.signInMethodService', 'platformWebApp.dialogService', 'platformWebApp.externalSignInStorage',
        function ($transitions, mainMenuService, metaFormsService, widgetService, $state, authService, $window, signInMethodService, dialogService, externalSignInStorage) {
            //Register module in main menu
            var menuItem = {
                path: 'configuration/security',
                icon: 'fas fa-key',
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
                        name: "status",
                        templateUrl: "statusSelector.html",
                        priority: 3
                    },
                    {
                        name: "accountType",
                        templateUrl: "accountTypeSelector.html",
                        priority: 4
                    },
                    {
                        name: "accountInfo",
                        templateUrl: "accountInfo.html",
                        priority: 5
                    }
                ]);

            // Sign-in log record fields, registered as a metaForm so a module can add its own
            // without touching this blade. Values are filled in by the detail controller; every
            // field is read-only because audit rows are written once and never edited.
            metaFormsService.registerMetaFields("signInLogDetails",
                [
                    { name: "createdDate", title: "platform.blades.sign-in-log.labels.date", valueType: "DateTime", priority: 10 },
                    { name: "userName", title: "platform.blades.sign-in-log.labels.user-name", valueType: "ShortText", priority: 20 },
                    // No "succeeded" field: the outcome badge at the top of the blade already says it.
                    { name: "failureReason", title: "platform.blades.sign-in-log.labels.failure-reason", valueType: "ShortText", priority: 40 },
                    { name: "signInType", title: "platform.blades.sign-in-log.labels.sign-in-type", valueType: "ShortText", priority: 50 },
                    { name: "provider", title: "platform.blades.sign-in-log.detail.provider", valueType: "ShortText", priority: 60 },
                    { name: "operatorUserName", title: "platform.blades.sign-in-log.detail.operator-name", valueType: "ShortText", priority: 70 },
                    { name: "operatorUserId", title: "platform.blades.sign-in-log.detail.operator-id", valueType: "ShortText", priority: 80 },
                    { name: "userId", title: "platform.blades.sign-in-log.detail.user-id", valueType: "ShortText", priority: 90 },
                    { name: "memberId", title: "platform.blades.sign-in-log.detail.member-id", valueType: "ShortText", priority: 100 },
                    { name: "ipAddress", title: "platform.blades.sign-in-log.labels.ip-address", valueType: "ShortText", priority: 110 },
                    { name: "host", title: "platform.blades.sign-in-log.detail.host", valueType: "ShortText", priority: 115 },
                    { name: "userAgent", title: "platform.blades.sign-in-log.detail.user-agent", valueType: "LongText", priority: 120 },
                    { name: "clientId", title: "platform.blades.sign-in-log.detail.client-id", valueType: "ShortText", priority: 130 },
                    { name: "sessionId", title: "platform.blades.sign-in-log.detail.session-id", valueType: "ShortText", priority: 140 },
                    { name: "storeId", title: "platform.blades.sign-in-log.filter.store", valueType: "ShortText", priority: 150 },
                    { name: "storeName", title: "platform.blades.sign-in-log.detail.store-name", valueType: "ShortText", priority: 160 },
                    { name: "organizationName", title: "platform.blades.sign-in-log.labels.organization", valueType: "ShortText", priority: 170 },
                    { name: "organizationId", title: "platform.blades.sign-in-log.detail.organization-id", valueType: "ShortText", priority: 180 }
                ]);

            //Register widgets
            widgetService.registerWidget({
                controller: 'platformWebApp.accountRolesWidgetController',
                template: '$(Platform)/Scripts/app/security/widgets/accountRolesWidget.tpl.html',
            }, 'accountDetail');
            widgetService.registerWidget({
                controller: 'platformWebApp.changeLog.operationsWidgetController',
                template: '$(Platform)/Scripts/app/changeLog/widgets/operations-widget.tpl.html'
            }, 'accountDetail');
            widgetService.registerWidget({
                controller: 'platformWebApp.accountApiWidgetController',
                template: '$(Platform)/Scripts/app/security/widgets/accountApiWidget.tpl.html',
            }, 'accountDetail');
            widgetService.registerWidget({
                controller: 'platformWebApp.accountSessionsWidgetController',
                template: '$(Platform)/Scripts/app/security/widgets/accountSessionsWidget.html'
            }, 'accountDetail');
            widgetService.registerWidget({
                controller: 'platformWebApp.accountSignInLogWidgetController',
                template: '$(Platform)/Scripts/app/security/widgets/accountSignInLogWidget.html',
                permission: 'platform:security:sign_in_log:read'
            }, 'accountDetail');

            signInMethodService.onSignInMethodsLoaded(function (signInMethods) {
                _.each(signInMethods, function (signInMethod) {
                    switch (signInMethod.kind) {
                        case 'External':
                            signInMethodService.extendSignInMethod(signInMethod.name, {
                                keyTemplateUrl: '$(Platform)/Scripts/app/userProfile/templates/external-sign-in-key.html',
                                signIn: function () {
                                    externalSignInStorage.set({ providerType: signInMethod.name });
                                    $window.location.href = 'externalsignin?authenticationType=' + signInMethod.name;
                                }
                            });

                            break;
                        case 'Password':
                            signInMethodService.extendSignInMethod(signInMethod.name, {
                                keyTemplateUrl: '$(Platform)/Scripts/app/userProfile/templates/password-sign-in-key.html',
                                noKeysTitle: 'platform.blades.sign-in-methods-list.labels.password-not-set',
                                createKey: function () {
                                    const dialog = {
                                        id: 'changePasswordDialog',
                                        requireCurrentPassword: false
                                    };

                                    dialogService.showDialog(dialog, '$(Platform)/Scripts/app/security/dialogs/changePasswordDialog.tpl.html', 'platformWebApp.changePasswordDialog');

                                    return dialog.instance.result;
                                }
                            });

                            break;
                    }
                });
            });


            $transitions.onBefore({ to: 'workspace.**' }, function (transition) {
                if (!authService.isAuthenticated) {
                    return;
                }

                // Prevent transition to workspace if the server denied admin UI access.
                if (!authService.canAccessAdminUI) {
                    return transition.router.stateService.target('contact-admin');
                }

                // Prevent transition to workspace if password expired
                if (authService.passwordExpired) {
                    return transition.router.stateService.target('changePasswordDialog');
                }
            });
        }]);
