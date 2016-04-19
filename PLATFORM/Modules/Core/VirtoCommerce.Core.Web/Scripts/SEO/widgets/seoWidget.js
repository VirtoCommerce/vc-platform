angular.module('virtoCommerce.coreModule.seo')
.controller('virtoCommerce.coreModule.seo.seoWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.coreModule.seoApi', function ($scope, bladeNavigationService, seoApi) {
    var blade = $scope.blade;
    var promise;

    $scope.openSeoBlade = function () {
        if (promise)
            promise.then(openBlade);
        else
            openBlade();
    };

    function openBlade(duplicates) {
        var newBlade = {
            id: "seoList",
            title: blade.title,
            duplicates: duplicates,
            objectType: $scope.widget.objectType,
            seoContainerObject: $scope.data,
            fixedStoreId: $scope.widget.getFixedStoreId ? $scope.widget.getFixedStoreId(blade) : undefined,
            defaultContainerId: $scope.widget.getDefaultContainerId(blade),
            languages: $scope.widget.getLanguages(blade),
            updatePermission: blade.updatePermission,
            controller: 'virtoCommerce.coreModule.seo.seoListController',
            template: 'Modules/$(VirtoCommerce.Core)/Scripts/SEO/blades/seo-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    $scope.$watch('data', function (data) {
        if (data && $scope.widget.getDefaultContainerId(blade)) {
            promise = seoApi.query({ objectId: data.id, objectType: $scope.widget.objectType }).$promise;
            promise.then(function (promiseData) {
                $scope.widget.UIclass = _.any(promiseData) ? 'error' : '';
            });
        }
    });
}]);
