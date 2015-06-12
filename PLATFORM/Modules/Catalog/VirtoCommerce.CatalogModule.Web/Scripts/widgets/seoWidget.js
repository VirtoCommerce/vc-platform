angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.seoWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.widget.blade;

    $scope.openSeoBlade = function () {
        var newBlade = {
            id: "seoDetail",
            seoUrlKeywordType: getSeoUrlKeywordType(),
            isNew: blade.isNew,
            title: blade.title,
            controller: 'virtoCommerce.catalogModule.seoDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/seo-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, blade);
    };

    function getSeoUrlKeywordType() {
        switch (blade.controller) {
            case "virtoCommerce.catalogModule.categoryDetailController":
            case "virtoCommerce.catalogModule.newCategoryWizardController":
                return 0; // SeoUrlKeywordTypes enum
            case "virtoCommerce.catalogModule.itemDetailController":
            case "virtoCommerce.catalogModule.newProductWizardController":
                return 1;
            case "virtoCommerce.catalogModule.catalogDetailController":
            case "virtoCommerce.catalogModule.virtualCatalogDetailController":
                return 3;
            default:
                return null;
        }
    }

}]);
