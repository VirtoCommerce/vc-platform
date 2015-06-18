angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.loginOnBehalfListController', ['$scope', '$window', 'virtoCommerce.storeModule.stores', 'platformWebApp.bladeNavigationService', function ($scope, $window, stores, bladeNavigationService) {
    $scope.selectedNodeId = null;

    $scope.blade.refresh = function () {
        stores.query({}, function (data) {
            $scope.blade.isLoading = false;
            $scope.blade.currentEntities = data;
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }

    $scope.selectNode = function (store) {
        $scope.selectedNodeId = store.id;

        if (!store.secureUrl || !store.defaultLanguage) {
            var newBlade = {
                id: 'storeDetails',
                currentEntityId: store.id,
                title: store.name,
                subtitle: 'Store details',
                controller: 'virtoCommerce.storeModule.storeDetailController',
                template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/store-detail.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, $scope.blade);
        } else {
            // https://{store_secure_url}/{locale}/{store_code}?uid={customer_id}
            var url = store.secureUrl + '/' + store.defaultLanguage + '/' + store.id + '?uid=' + $scope.blade.currentEntityId;
            $window.open(url, '_blank');
        }
    }

    $scope.blade.headIcon = 'fa fa-key';

    $scope.blade.refresh();
}]);
