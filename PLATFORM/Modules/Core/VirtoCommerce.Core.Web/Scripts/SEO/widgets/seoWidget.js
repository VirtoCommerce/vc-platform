angular.module('virtoCommerce.coreModule.seo')
.controller('virtoCommerce.coreModule.seo.seoWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.coreModule.seoApi', function ($scope, bladeNavigationService, seoApi) {
    var blade = $scope.blade;

    var promise = seoApi.query({ objectId: $scope.data.id, objectType: $scope.widget.objectType }).$promise;

    function searchForDulicates() {
        promise.then(function (promiseData) {
            $scope.duplicates = promiseData;
        });
    }

    $scope.openSeoBlade = function () {
        promise.then(function (promiseData) {
            var newBlade = {
                id: "seoList",
                title: blade.title,
                duplicates: promiseData,
                seoContainerObject: $scope.data,
                fixedStoreId: $scope.widget.getFixedStoreId ? $scope.widget.getFixedStoreId(blade) : undefined,
                languages: $scope.widget.getLanguages(blade),
                updatePermission: blade.updatePermission,
                controller: 'virtoCommerce.coreModule.seo.seoListController',
                template: 'Modules/$(VirtoCommerce.Core)/Scripts/SEO/blades/seo-list.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        });
    };

    searchForDulicates();
}]);
