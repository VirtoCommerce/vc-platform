angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeSettingsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.widget.blade;
    
    $scope.openBlade = function () {
        var newBlade = {
            id: "storeChildBlade",
            currentEntities: blade.currentEntity.settings,
            title: blade.title,
            subtitle: 'Settings',
            controller: 'virtoCommerce.storeModule.storeSettingsListController',
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/store-settings-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);