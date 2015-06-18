angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.seoWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

    $scope.openSeoBlade = function () {
        var newBlade = {
            id: "seoDetail",
            seoUrlKeywordType: 2, // SeoUrlKeywordTypes enum
            isNew: blade.isNew,
            title: blade.title,
            controller: 'virtoCommerce.storeModule.seoDetailController',
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/seo-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);
