angular.module('virtoCommerce.coreModule.fulfillment')
.controller('fulfillmentListController', ['$scope', 'fulfillments', 'bladeNavigationService',
function ($scope, fulfillments, bladeNavigationService) {
    var selectedNode = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        //$scope.blade.parentWidget.refresh().$promise.then(function (results) {
        fulfillments.query({}, function (results) {
            $scope.blade.isLoading = false;
            $scope.blade.currentEntities = results;

            if (selectedNode != null) {
                //select the node in the new list
                angular.forEach(results, function (node) {
                    if (selectedNode.id === node.id) {
                        selectedNode = node;
                    }
                });
            }

            return results;
        });
    };

    function showDetailBlade(node, title) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.id;

        var newBlade = {
            id: 'fulfillmentDetail',
            currentEntityId: selectedNode.id,
            currentEntity: selectedNode,
            title: title,
            subtitle: 'Edit Fulfillment center',
            controller: 'fulfillmentCenterDetailController',
            template: 'Modules/$(VirtoCommerce.Core)/Scripts/fulfillment/blades/fulfillment-center-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.selectNode = function (node) {
        showDetailBlade(node, node.name);
    };

    $scope.blade.onClose = function (closeCallback) {
      closeChildrenBlades();
      closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
            });
    }

    $scope.bladeHeadIco = 'fa fa-wrench';
    $scope.bladeToolbarCommands = [
      {
          name: "Refresh", icon: 'fa fa-refresh',
          executeMethod: function () {
              $scope.blade.refresh();
          },
          canExecuteMethod: function () {
              return true;
          }
      },
        {
            name: "Add", icon: 'fa fa-plus',
            executeMethod: function () {
                showDetailBlade({ maxReleasesPerPickBatch: 20, pickDelay: 30 }, 'New Fulfillment center');
            },
            canExecuteMethod: function () {
                return true;
            }
        }
    ];

    // actions on load
    $scope.blade.title = 'Fulfillment centers',
    $scope.blade.subtitle = 'Manage Fulfillment centers',
    $scope.blade.refresh();
}]);