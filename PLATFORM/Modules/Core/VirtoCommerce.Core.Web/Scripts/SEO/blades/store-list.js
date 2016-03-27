angular.module('virtoCommerce.coreModule.seo')
.controller('virtoCommerce.coreModule.seo.storeListController', ['$scope', 'platformWebApp.uiGridHelper', 'virtoCommerce.storeModule.stores', 'platformWebApp.bladeNavigationService', function ($scope, uiGridHelper, stores, bladeNavigationService) {
    var blade = $scope.blade;
    $scope.selectedNodeId = null; // need to initialize to null
    var promise = stores.query().$promise;

    function startBladeInitialization(parentEntity) {
        if (parentEntity) {
            // set currentEntity to reference it from child blade.
            blade.currentEntity = parentEntity;

            blade.refresh();
        }
    }

    blade.refresh = function () {
        promise.then(function (promiseData) {
            var sortedStores = _.sortBy(promiseData, 'name');
            sortedStores.splice(0, 0, { name: 'default SEO (not a store)' });
            _.each(sortedStores, function (x) {
                x.seoInfo = _.find(blade.currentEntity.seoInfos, function (info) { return info.storeId === x.id; });
            });
            $scope.stores = _.filter(sortedStores, function (x) { return !x.seoInfo });
            blade.currentEntities = _.filter(sortedStores, function (x) { return x.seoInfo });
            blade.isLoading = false;
        });
    };

    $scope.addNewSeo = function () {
        if (blade.newStore) {
            blade.selectNode(blade.newStore);
            blade.newStore = null;
        }
    };

    blade.selectNode = function (node) {
        $scope.selectedNodeId = node.id;

        var newBlade = {
            id: 'seoDetails',
            store: node,
            parentRefresh: blade.refresh,
            updatePermission: blade.updatePermission,
            controller: 'virtoCommerce.coreModule.seo.seoDetailController',
            template: 'Modules/$(VirtoCommerce.Core)/Scripts/SEO/blades/seo-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        uiGridHelper.initialize($scope, gridOptions);
    };

    blade.headIcon = 'fa-globe';
    blade.subtitle = 'core.blades.store-list.subtitle';

    $scope.$watch('blade.parentBlade.currentEntity', startBladeInitialization); // for category
    $scope.$watch('blade.parentBlade.item', startBladeInitialization);          // for item
}]);
