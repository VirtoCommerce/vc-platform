angular.module('platformWebApp')
    .controller('platformWebApp.pushNotificationsHistoryController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.pushNotificationTemplateResolver', 'platformWebApp.pushNotifications', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper',
        function ($scope, bladeNavigationService, eventTemplateResolver, notifications, bladeUtils, uiGridHelper) {
            var blade = $scope.blade;

            blade.refresh = function () {
                blade.isLoading = true;

                var criteria = {
                    sort: uiGridHelper.getSortExpression($scope),
                    skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                    take: $scope.pageSettings.itemsPerPageCount
                };

                notifications.query(criteria, function (data) {
                    $scope.pageSettings.totalItems = data.totalCount;
                    blade.currentEntities = data.notifyEvents;
                    blade.isLoading = false;
                }, function (error) {
                    bladeNavigationService.setError('Error ' + error.status, blade);
                });
            };

            blade.selectNode = function (node) {
                $scope.selectedNodeId = node.id;
                const notificationTemplate = eventTemplateResolver.resolve(node, 'history');
                var action = notificationTemplate.action;
                if (action) {
                    action(node);
                }
            };

            blade.toolbarCommands = [
                {
                    name: "platform.commands.refresh",
                    icon: 'fa fa-refresh',
                    executeMethod: blade.refresh,
                    canExecuteMethod: function () {
                        return true;
                    }
                }];

            // ui-grid
            $scope.setGridOptions = function (gridOptions) {
                uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
                    $scope.gridApi = gridApi;
                    uiGridHelper.bindRefreshOnSortChanged($scope);
                });
                bladeUtils.initializePagination($scope);
            };
        }]);
