angular.module('platformWebApp').directive('vaHeaderLicenseWidget', ['$state',
    function ($state) {

        return {
            restrict: 'E',
            replace: true,
            transclude: true,
            templateUrl: '$(Platform)/Scripts/app/licensing/headerLicenseWidget.tpl.html',
            link: function (scope) {

                scope.showLicense = function () {
                    $state.go('workspace.appLicense');
                };
            }
        }
    }]);
