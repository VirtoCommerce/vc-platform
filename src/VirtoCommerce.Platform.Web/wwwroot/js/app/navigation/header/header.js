angular.module('platformWebApp').directive('vaHeader', ["$state",
    function ($state) {
        return {
            restrict: 'E',
            replace: true,
            transclude: true,
            templateUrl: '$(Platform)/Scripts/app/navigation/header/header.tpl.html',
            link: function (scope) {
                //tooltip popup delay for all header widgets
                scope.tooltipPopupDelay = 2000;

                scope.manageSettings = function () {
                    $state.go('workspace.modulesSettings');
                }
            }
        }
    }]);
