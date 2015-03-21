angular.module('virtoCommerce.storeModule.blades')
.controller('storePaymentsListController', ['$scope', 'bladeNavigationService', 'paymentgateways', function ($scope, bladeNavigationService, paymentgateways) {
    function getAvailableGateways() {
        paymentgateways.query({}, function (data) {
            $scope.blade.isLoading = false;
            $scope.availableGateways = data;
        });
    }

    $scope.optionChanged = function (data) {
        var idx = $scope.blade.currentEntities.indexOf(data);
        if (idx > -1) {
            $scope.blade.currentEntities.splice(idx, 1);
        } else {
            $scope.blade.currentEntities.push(data);
        }
    }

    $scope.bladeHeadIco = 'fa fa-archive';

    $scope.$watch('blade.parentBlade.currentEntity.paymentGateways', function (currentEntities) {
        $scope.blade.currentEntities = currentEntities;
    });

    
    getAvailableGateways();
}]);
