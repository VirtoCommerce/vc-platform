angular.module('platformWebApp')
.controller('platformWebApp.entitySettingsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;
    
    $scope.openBlade = function () {
        var newBlade = {
            id: 'entitySettingList',
            controller: 'platformWebApp.entitySettingListController',
            template: 'Scripts/app/settings/blades/entitySetting-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);