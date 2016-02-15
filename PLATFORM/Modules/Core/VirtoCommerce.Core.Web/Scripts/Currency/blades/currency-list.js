angular.module('virtoCommerce.coreModule.currency')
.controller('virtoCommerce.coreModule.currency.currencyListController', ['$scope', 'virtoCommerce.coreModule.currency.currencyApi', 'platformWebApp.bladeNavigationService',
function ($scope, currencyApi, bladeNavigationService) {
    var blade = $scope.blade;

    blade.refresh = function (parentRefresh) {
        blade.isLoading = true;

        currencyApi.query({}, function (results) {
            blade.isLoading = false;
            blade.currentEntities = results;

            if (parentRefresh && blade.parentRefresh) {
                blade.parentRefresh(results);
            }
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
        });
    };

    blade.setSelectedId = function (selectedNodeId) {
        $scope.selectedNodeId = selectedNodeId;
    };

    function showDetailBlade(bladeData) {
        var newBlade = {
            id: 'currencyDetail',
            // data: node,            
            controller: 'virtoCommerce.coreModule.currency.currencyDetailController',
            template: 'Modules/$(VirtoCommerce.Core)/Scripts/currency/blades/currency-detail.tpl.html'
        };
        angular.extend(newBlade, bladeData);
        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.selectNode = function (node) {
        blade.setSelectedId(node.code);
        showDetailBlade({ data: node });
    };

    blade.headIcon = 'fa-money';
    blade.toolbarCommands = [
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
                blade.setSelectedId(null);
                showDetailBlade({ isNew: true });
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'core:currency:create'
        }
    ];

    // actions on load
    blade.title = 'core.blades.currency-list.title',
    blade.subtitle = 'core.blades.currency-list.subtitle',
    blade.refresh();
}]);