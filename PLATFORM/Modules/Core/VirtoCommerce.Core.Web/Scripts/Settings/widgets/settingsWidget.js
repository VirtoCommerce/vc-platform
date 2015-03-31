angular.module('virtoCommerce.coreModule.settings')
.controller('settingsWidgetController', ['$scope', 'bladeNavigationService', 'settings', function ($scope, bladeNavigationService, settings) {
    var blade = $scope.widget.blade;
    
    $scope.widget.refresh = function () {
        $scope.currentNumberInfo = '...';
        return settings.getSettings({ id: blade.currentEntity.id }, function (results) {
            $scope.currentNumberInfo = results.length;
        });
    }

    $scope.openBlade = function () {
        var newBlade = {
            id: 'moduleSettingsSection',
            moduleId: blade.currentEntity.id,
            // parentWidget: $scope.widget,
            title: 'Module settings',
            //subtitle: '',
            controller: 'settingsDetailController',
            template: 'Modules/$(VirtoCommerce.Core)/Scripts/Settings/blades/settings-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.widget.refresh();
}]);