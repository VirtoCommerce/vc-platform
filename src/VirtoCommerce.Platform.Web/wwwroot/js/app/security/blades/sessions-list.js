angular.module('platformWebApp')
    .controller('platformWebApp.sessionsListController',
        ['$scope', 'platformWebApp.accounts', 'platformWebApp.bladeUtils', 'platformWebApp.dialogService', 'platformWebApp.uiGridHelper',
            function ($scope, accounts, bladeUtils, dialogService, uiGridHelper) {
                $scope.uiGridConstants = uiGridHelper.uiGridConstants;
                var blade = $scope.blade;
                blade.title = "platform.blades.sessions-list.title";

                var filter = $scope.filter = {};

                blade.refresh = function () {
                    blade.isLoading = true;

                    accounts.searchSessions(
                        { userId: blade.userId },
                        {
                            keyword: filter.keyword,
                            skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                            take: $scope.pageSettings.itemsPerPageCount
                        },
                        function (data) {
                            blade.isLoading = false;

                            $scope.pageSettings.totalItems = data.totalCount;
                            blade.currentEntities = data.results;

                            if (blade.refreshSessionsCountCallback && angular.isFunction(blade.refreshSessionsCountCallback)) {
                                blade.refreshSessionsCountCallback(data.totalCount);
                            }
                        });
                };

                $scope.terminate = function (session) {
                    var dialog = {
                        id: "confirmTerminateSession",
                        title: "platform.dialogs.session-terminate.title",
                        message: "platform.dialogs.session-terminate.message",
                        callback: function (remove) {
                            if (remove) {
                                accounts.terminateSession({ userId: blade.userId, id: session.id }, null, () => {
                                    blade.refresh();
                                });
                            }
                        }
                    }
                    dialogService.showConfirmationDialog(dialog);
                }

                blade.terminateAll = function () {
                    var dialog = {
                        id: "confirmTerminateAllSessions",
                        title: "platform.dialogs.sessions-terminate-all.title",
                        message: "platform.dialogs.sessions-terminate-all.message",
                        callback: function (remove) {
                            if (remove) {
                                accounts.terminateAllSessions({ userId: blade.userId }, null, () => {
                                    blade.refresh();
                                });
                            }
                        }
                    }
                    dialogService.showConfirmationDialog(dialog);
                }

                blade.headIcon = 'fas fa-key';

                blade.toolbarCommands = [
                    {
                        name: "platform.commands.refresh", icon: 'fa fa-refresh',
                        executeMethod: blade.refresh,
                        canExecuteMethod: function () {
                            return true;
                        }
                    },
                    {
                        name: "platform.commands.sessions-terminate-all", icon: 'fa fa-trash',
                        executeMethod: blade.terminateAll,
                        canExecuteMethod: function () {
                            return blade.currentEntities && blade.currentEntities.length;
                        }
                    },
                ];

                filter.criteriaChanged = function () {
                    if ($scope.pageSettings.currentPage > 1) {
                        $scope.pageSettings.currentPage = 1;
                    } else {
                        blade.refresh();
                    }
                };

                // ui-grid
                $scope.setGridOptions = function (gridOptions) {
                    uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
                        uiGridHelper.bindRefreshOnSortChanged($scope);
                    });
                    bladeUtils.initializePagination($scope);
                };
            }]);
