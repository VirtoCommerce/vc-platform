angular.module('platformWebApp')
.controller('platformWebApp.entitySettingsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

    $scope.openBlade = function () {
        $scope.openUnifiedBlade();
    };

    function getEntity() {
        return blade.currentEntity || {};
    }

    // Extract short type name from fully-qualified objectType
    // e.g. "VirtoCommerce.StoreModule.Core.Model.Store" -> "Store"
    function getTenantType() {
        var fullType = getEntity().objectType || getEntity().typeName;
        if (!fullType) {
            return undefined;
        }
        var parts = fullType.split('.');
        return parts[parts.length - 1];
    }

    function getTenantId() {
        return getEntity().id;
    }

    function getSettingNames() {
        var settings = getEntity().settings;
        return settings ? _.pluck(settings, 'name') : undefined;
    }

    $scope.openUnifiedBlade = function () {
        var newBlade = {
            id: 'entitySettingList',
            title: 'platform.blades.settings-unified.title',
            updatePermission: blade.updatePermission,
            tenantType: getTenantType(),
            tenantId: getTenantId(),
            settingNames: getSettingNames(),
            isEntityMode: true,
            isExpandable: true,
            controller: 'platformWebApp.settingsUnifiedController',
            template: '$(Platform)/Scripts/app/settings/blades/settings-unified.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);
