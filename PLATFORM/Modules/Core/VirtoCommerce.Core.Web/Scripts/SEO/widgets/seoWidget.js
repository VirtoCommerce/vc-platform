angular.module('virtoCommerce.coreModule.seo')
.controller('virtoCommerce.coreModule.seo.seoWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

    $scope.openSeoBlade = function () {
        var newBlade = {
            id: "seoList",
            title: blade.title,
            seoContainerObject: $scope.data,
            fixedStoreId: $scope.widget.getFixedStoreId ? $scope.widget.getFixedStoreId(blade) : undefined,
            languages: $scope.widget.getLanguages(blade),
            updatePermission: blade.updatePermission,
            controller: 'virtoCommerce.coreModule.seo.seoListController',
            template: 'Modules/$(VirtoCommerce.Core)/Scripts/SEO/blades/seo-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

}]);
