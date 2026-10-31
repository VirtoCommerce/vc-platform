angular.module('platformWebApp')
    .directive('vaHeaderUserProfileWidget', [
        '$document', '$state', 'platformWebApp.authService', 'platformWebApp.userProfileIconService',
        function ($document, $state, authService, userProfileIconService) {

            return {
                restrict: 'E',
                replace: true,
                transclude: true,
                scope: {},
                templateUrl: '$(Platform)/Scripts/app/security/login/headerUserProfileWidget.tpl.html',
                link: function (scope) {
                    scope.dropDownOpened = false;
                    scope.userLogin = '';
                    scope.fullName = '';
                    scope.userType = '';
                    scope.isAdministrator = false;
                    scope.userIconUrl = userProfileIconService.getUserIconUrl();

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

                    scope.$watch(function () {
                        return userProfileIconService.iconUrlChanged;
                    }, function () {
                        scope.userIconUrl = userProfileIconService.getUserIconUrl();
                    });

                    $document.bind('click', handleClickEvent);

                    scope.$on('$destroy', function () {
                        $document.unbind('click', handleClickEvent);
                    });
                }
            }
        }]);
