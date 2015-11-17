angular.module('platformWebApp')
.controller('platformWebApp.changeLog.operationsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

    $scope.openBlade = function () {
        var newBlade = {
            id: "changesChildBlade",
            currentEntities: blade.currentEntity.operationsLog,
            headIcon: blade.headIcon,
            title: blade.title,
            subtitle: 'platform.widgets.operations.blade-subtitle',
            controller: 'platformWebApp.changeLog.operationListController',
            template: '$(Platform)/Scripts/app/changeLog/blades/operation-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);