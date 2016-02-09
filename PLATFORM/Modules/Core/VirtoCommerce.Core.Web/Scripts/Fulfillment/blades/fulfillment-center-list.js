angular.module('virtoCommerce.coreModule.fulfillment')
.controller('virtoCommerce.coreModule.fulfillment.fulfillmentListController', ['$scope', 'virtoCommerce.coreModule.fulfillment.fulfillments', 'platformWebApp.bladeNavigationService',
function ($scope, fulfillments, bladeNavigationService) {
    var selectedNode = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

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
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
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
            subtitle: 'core.blades.fulfillment-center-detail.subtitle',
            controller: 'virtoCommerce.coreModule.fulfillment.fulfillmentCenterDetailController',
            template: 'Modules/$(VirtoCommerce.Core)/Scripts/fulfillment/blades/fulfillment-center-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.selectNode = function (node) {
        showDetailBlade(node, node.name);
    };

    $scope.blade.headIcon = 'fa-wrench';
    $scope.blade.toolbarCommands = [
      {
          name: "platform.commands.refresh", icon: 'fa fa-refresh',
          executeMethod: blade.refresh,
          canExecuteMethod: function () {
              return true;
          }
      },
        {
            name: "platform.commands.add", icon: 'fa fa-plus',
            executeMethod: function () {
                showDetailBlade({ maxReleasesPerPickBatch: 20, pickDelay: 30 }, 'New Fulfillment center');
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'core:fulfillment:create'
        }
    ];

    // actions on load
    $scope.blade.title = 'core.blades.fulfillment-center-list.title',
    $scope.blade.subtitle = 'core.blades.fulfillment-center-list.subtitle',
    $scope.blade.refresh();
}]);