angular.module('virtoCommerce.coreModule.fulfillment')
.controller('fulfillmentWidgetController', ['$scope', 'bladeNavigationService', 'fulfillments', function ($scope, bladeNavigationService, fulfillments) {
    var blade = $scope.widget.blade;
    $scope.showWidget = blade.currentEntity.id == 'VirtoCommerce.Core';

    $scope.widget.refresh = function () {
        $scope.currentNumberInfo = '...';
        return fulfillments.query({}, function (results) {
            $scope.currentNumberInfo = results.length;
        });
    }

    $scope.openBlade = function () {
        var newBlade = {
            id: 'fulfillmentCenterList',
            parentWidget: $scope.widget,
            controller: 'fulfillmentListController',
            template: 'Modules/$(VirtoCommerce.Core)/Scripts/fulfillment/blades/fulfillment-center-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    if ($scope.showWidget) {
        $scope.widget.refresh();
    }
}]);