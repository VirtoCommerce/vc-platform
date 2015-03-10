angular.module('virtoCommerce.coreModule.fulfillment.blades', [
   'virtoCommerce.coreModule.common.resources'
])
.controller('fulfillmentListController', ['$scope', 'fulfillments', 'bladeNavigationService',
function ($scope, fulfillments, bladeNavigationService) {
    var selectedNode = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        fulfillments.query({}, function (results) {
            $scope.blade.isLoading = false;

            $scope.objects = results;

            if (selectedNode != null) {
                //select the node in the new list
                angular.forEach(results, function (node) {
                    if (selectedNode.id === node.id) {
                        selectedNode = node;
                    }
                });
            }
        });
    };

    $scope.selectNode = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.id;

        var newBlade = {
            id: 'fulfillmentDetail',
            title: selectedNode.name,
            currentEntityId: selectedNode.id,
            controller: 'fulfillmentCenterDetailController',
            template: 'Modules/Core/VirtoCommerce.Core.Web/Scripts/fulfillment/blades/fulfillment-center-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.bladeHeadIco = 'fa fa-wrench';


    // actions on load
    $scope.blade.title= 'Fulfillment centers',
    $scope.blade.subtitle = 'Manage Fulfillment centers',
    $scope.blade.refresh();
}]);