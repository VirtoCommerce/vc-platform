angular.module('platformWebApp')
.controller('platformWebApp.notificationsJournalController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.notifications', 'platformWebApp.bladeUtils', 'platformWebApp.dialogService', 'uiGridConstants', 'platformWebApp.uiGridHelper',
    function ($scope, bladeNavigationService, notifications, bladeUtils, dialogService, uiGridConstants, uiGridHelper) {
        var blade = $scope.blade;
        $scope.uiGridConstants = uiGridConstants;

        blade.refresh = function () {
            notifications.getNotificationJournalList({
                objectId: blade.objectId,
                objectTypeId: blade.objectTypeId,
                start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                count: $scope.pageSettings.itemsPerPageCount
            }, function (data) {
                blade.currentEntities = data.notifications;
                $scope.pageSettings.totalItems = data.totalCount;
                blade.isLoading = false;
            });
        };

        $scope.selectNode = function (data) {
            $scope.selectedNodeId = data.id;

            var newBlade = {
                id: 'notificationDetails',
                title: 'platform.blades.notification-journal-details.title',
                currentNotificationId: data.id,
                currentEntity: data,
                controller: 'platformWebApp.notificationsJournalDetailtsController',
                template: '$(Platform)/Scripts/app/notifications/blades/notification-journal-details.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };

        $scope.stopNotifications = function (list) {
            blade.isLoading = true;
            notifications.stopSendingNotifications(_.pluck(list, 'id'), blade.refresh);
        };

        blade.toolbarCommands = [
            {
                name: "platform.commands.refresh", icon: 'fa fa-refresh',
                executeMethod: blade.refresh,
                canExecuteMethod: function () { return true; }
            },
            {
                name: "platform.commands.stop-sending", icon: 'fa fa-stop',
                executeMethod: function () {
                    $scope.stopNotifications($scope.gridApi.selection.getSelectedRows());
                },
                canExecuteMethod: function () {
                    return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
                }
            }
        ];

        // ui-grid
        $scope.setGridOptions = function (gridOptions) {
            uiGridHelper.initialize($scope, gridOptions);
            bladeUtils.initializePagination($scope);
        };
    }]);