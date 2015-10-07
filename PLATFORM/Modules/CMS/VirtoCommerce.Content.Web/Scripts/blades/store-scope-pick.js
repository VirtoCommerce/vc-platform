angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.storeScopePickController', ['$scope', 'virtoCommerce.storeModule.stores', function ($scope, stores) {
    var blade = $scope.blade;

    function initializeBlade() {
        stores.query({}, function (data) {
            blade.isLoading = false;
            
            _.each(blade.currentEntity.assignedScopes, function (x) {
                var store = _.find(data, function (y) { return x.scope === y.id; });
                if (store) {
                    store.$selected = true;
                }
            });

            blade.currentEntities = data;
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
        });
    }

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    };

    $scope.isValid = function () {
        return _.any(blade.currentEntities, function (x) { return x.$selected; });
    };

    $scope.saveChanges = function () {
        var selection = _.map(_.where(blade.currentEntities, { $selected: true }), function (x) {
          	return angular.extend({ scope: x.id, label: x.name }, blade.currentEntity.scopeOriginal);
        });
        blade.onChangesConfirmedFn(selection);
        $scope.bladeClose();
    };

    blade.headIcon = 'fa-key';
    initializeBlade();
}]);
