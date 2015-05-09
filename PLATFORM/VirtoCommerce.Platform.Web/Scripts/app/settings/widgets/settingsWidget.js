angular.module('platformWebApp')
.controller('platformWebApp.settingsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', function ($scope, bladeNavigationService, settings) {
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
            controller: 'platformWebApp.settingsDetailController',
            template: 'Scripts/app/settings/blades/settings-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.widget.refresh();
}]);