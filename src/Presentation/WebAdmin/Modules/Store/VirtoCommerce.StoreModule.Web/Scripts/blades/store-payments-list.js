angular.module('virtoCommerce.storeModule.blades')
.controller('storePaymentsListController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    function getAvailableGateways() {
        return ['CreditCard', 'Klarna', 'Mes', 'Paypal'];
    }

    $scope.optionChanged = function (data) {
        var idx = $scope.blade.currentEntities.indexOf(data);
        if (idx > -1) {
            $scope.blade.currentEntities.splice(idx, 1);
        } else {
            $scope.blade.currentEntities.push(data);
        }
    }
    
    $scope.$watch('blade.parentBlade.currentEntity.paymentGateways', function (currentEntities) {
        $scope.blade.currentEntities = currentEntities;
    });

    $scope.blade.isLoading = false;
    $scope.availableGateways = getAvailableGateways();
}]);
