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

    $scope.openUnifiedBlade = function () {
        var newBlade = {
            id: 'entitySettingList',
            updatePermission: blade.updatePermission,
            tenantType: blade.currentEntity && blade.currentEntity.objectType,
            tenantId: blade.currentEntity && blade.currentEntity.id,
            settingNames: blade.currentEntity && blade.currentEntity.settings
                ? _.pluck(blade.currentEntity.settings, 'name')
                : undefined,
            isSavingToParentObject: true,
            isExpandable: true,
            controller: 'platformWebApp.settingsUnifiedController',
            template: '$(Platform)/Scripts/app/settings/blades/settings-unified.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);