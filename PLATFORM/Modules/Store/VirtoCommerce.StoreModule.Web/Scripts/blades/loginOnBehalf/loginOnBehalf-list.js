angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.loginOnBehalfListController', ['$scope', '$window', 'virtoCommerce.storeModule.stores', 'platformWebApp.bladeNavigationService', function ($scope, $window, stores, bladeNavigationService) {
    var blade = $scope.blade;
    $scope.selectedNodeId = null;

    blade.refresh = function () {
        stores.queryLoginOnBehalfStores({ userId: blade.currentEntityId }, function (data) {
            blade.isLoading = false;
            blade.currentEntities = data;
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }

    $scope.selectNode = function (store) {
        $scope.selectedNodeId = store.id;

        if (!store.secureUrl || !store.defaultLanguage) {
            if (bladeNavigationService.checkPermission('store:update')) {
                var newBlade = {
                    id: 'storeDetails',
                    currentEntityId: store.id,
                    title: store.name,
                    controller: 'virtoCommerce.storeModule.storeDetailController',
                    template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/store-detail.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, blade);
            } else {
                bladeNavigationService.setError('Insufficient permission', blade);
            }
        } else {
            // {store_secure_url}/account/login?UserId={customer_id}
            var url = store.secureUrl + '/account/login?UserId=' + blade.currentEntityId;
            $window.open(url, '_blank');
        }
    }

    blade.headIcon = 'fa-key';

    blade.refresh();
}]);
