angular.module('platformWebApp')
    .directive('vaHeaderUserProfileWidget', ['$document', '$state', 'platformWebApp.authService', 'platformWebApp.dialogService', 'platformWebApp.login',
        function ($document, $state, authService, dialogService, loginResources) {

            return {
                restrict: 'E',
                replace: true,
                transclude: true,
                scope: {},
                templateUrl: '$(Platform)/Scripts/app/security/login/headerUserProfileWidget.tpl.html',
                link: function (scope) {

                    loginResources.getLoginTypes().$promise.then(function (loginTypes) {
                        loginTypes = _.filter(loginTypes, function (loginTypeFilter) {
                            return loginTypeFilter.authenticationType === "Password";
                        });

                        let loginType = _.first(_.sortBy(loginTypes, function (loginTypeSort) {
                            return loginTypeSort.priority;
                        }));

                        scope.isPasswordLoginEnabled = loginType.enabled;
                    });

                    scope.dropDownOpened = false;
                    scope.userLogin = '';
                    scope.fullName = '';
                    scope.userType = '';
                    scope.isAdministrator = false;

                    function handleClickEvent(event) {
                        var dropdownElement = $document.find('[userProfileWidget]');
                        var hadDropdownElement = $document.find('[userProfileWidget] .header__nav-item--opened');
                        if (scope.dropDownOpened && !(dropdownElement.is(event.target) || dropdownElement.has(event.target).length ||
                            hadDropdownElement.is(event.target) || hadDropdownElement.has(event.target).length)) {
                            scope.$apply(function () {
                                scope.dropDownOpened = false;
                            });
                        }
                    }

                    scope.logout = function () {
                        authService.logout();
                    };

                    scope.changePassword = () => {
                        var dialog = {
                            id: "changePasswordDialog"
                        };
                        dialogService.showDialog(dialog, '$(Platform)/Scripts/app/security/dialogs/changePasswordDialog.tpl.html', 'platformWebApp.changePasswordDialog');
                    };

                    scope.manageProfile = function () {
                        $state.go('workspace.userProfile');
                    };

                    scope.toggleDropDown = function () {
                        scope.dropDownOpened = !scope.dropDownOpened;
                    };

                    scope.$watch(function () {
                        return authService.userLogin;
                    }, function () {
                        scope.userLogin = authService.userLogin;
                        scope.fullName = authService.fullName;
                        scope.userType = authService.userType;
                        scope.isAdministrator = authService.isAdministrator;
                    });

                    $document.bind('click', handleClickEvent);

                    scope.$on('$destroy', function () {
                        $document.unbind('click', handleClickEvent);
                    });
                }
            }
        }]);
