angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberAccountsListController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.uiGridHelper', 'platformWebApp.bladeNavigationService', 'filterFilter',
function ($scope, dialogService, uiGridHelper, bladeNavigationService, filterFilter) {
    $scope.uiGridConstants = uiGridHelper.uiGridConstants;
    var blade = $scope.blade;

    function initializeBlade(data) {
        // blade.data = data;

        blade.currentEntities = angular.copy(data);
        blade.origEntity = data;
        blade.isLoading = false;
    }

    blade.selectNode = function (node) {
        if (bladeNavigationService.checkPermission('platform:security:read')) {
            $scope.selectedNodeId = node.userName;

            var newBlade = {
                id: 'listItemChild',
                data: node,
                title: node.userName,
                subtitle: blade.subtitle,
                controller: 'platformWebApp.accountDetailController',
                template: '$(Platform)/Scripts/app/security/blades/account-detail.tpl.html'
            };

            bladeNavigationService.showBlade(newBlade, blade);
        } else {
            bladeNavigationService.setError('Insufficient permission', blade);
        }
    };

    blade.headIcon = 'fa-key';

    blade.toolbarCommands = [
        //{
        //    name: "platform.commands.delete", icon: 'fa fa-trash-o',
        //    executeMethod: function () { deleteList($scope.gridApi.selection.getSelectedRows()); },
        //    canExecuteMethod: function () {
        //        return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
        //    },
        //    permission: 'platform:security:delete'
        //}
    ];

    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
            gridApi.grid.registerRowsProcessor($scope.singleFilter, 90);
        });
    };

    $scope.singleFilter = function (renderableRows) {
        var visibleCount = 0;
        renderableRows.forEach(function (row) {
            row.visible = _.any(filterFilter([row.entity], blade.searchText));
            if (row.visible) visibleCount++;
        });

        $scope.filteredEntitiesCount = visibleCount;
        return renderableRows;
    };

    $scope.$watch('blade.parentBlade.currentEntity.securityAccounts', initializeBlade);

    // on load:
    // $scope.$watch('blade.parentBlade.currentEntity.securityAccounts' gets fired    
}]);