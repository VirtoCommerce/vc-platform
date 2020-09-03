angular.module('platformWebApp')
    .directive('vaHeaderUserProfileWidget', ['$document', '$state', 'platformWebApp.authService',
        function ($document, $state, authService) {

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
                    }

                    scope.manageProfile = function () {
                        $state.go('workspace.userProfile');
                    }

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
