angular.module('platformWebApp')
.controller('platformWebApp.changeLog.operationsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

    $scope.openBlade = function () {
        var newBlade = {
            id: "changesChildBlade",
            currentEntities: blade.currentEntity.operationsLog,
            headIcon: blade.headIcon,
            title: blade.title,
            subtitle: 'Changes history',
            controller: 'platformWebApp.changeLog.operationListController',
            template: '$(Platform)/Scripts/app/changeLog/blades/operation-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);