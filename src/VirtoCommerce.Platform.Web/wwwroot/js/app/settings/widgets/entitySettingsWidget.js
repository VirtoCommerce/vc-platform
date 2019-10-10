angular.module('platformWebApp')
.controller('platformWebApp.entitySettingsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

    $scope.openBlade = function () {
        var newBlade = {
            id: 'entitySettingList',
            updatePermission: blade.updatePermission,
            controller: 'platformWebApp.entitySettingListController',
            template: '$(Platform)/Scripts/app/settings/blades/settings-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);