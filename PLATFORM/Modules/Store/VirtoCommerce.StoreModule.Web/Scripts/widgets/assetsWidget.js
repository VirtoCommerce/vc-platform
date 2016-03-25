angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.assetsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

    $scope.openBlade = function () {
        var newBlade = {
            id: "storeAssetList",
            subtitle: blade.title,
            controller: 'platformWebApp.assets.assetListController',
            template: '$(Platform)/Scripts/app/assets/blades/asset-list.tpl.html',
            currentEntity: { url: '/stores/' + blade.currentEntityId }
        };

        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);
