angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.dashboard.statisticsWidgetController', ['$scope', '$localStorage', '$state',
    function ($scope, $localStorage, $state) {
        $scope.$storage = $localStorage;

        $scope.openBlade = function () {
            $state.go('workspace.orderModule');
        };
    }]);
