angular.module('platformWebApp')
    .controller('platformWebApp.sessionsListController',
        ['$scope', 'platformWebApp.accounts', 'platformWebApp.bladeUtils', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.uiGridHelper',
            function ($scope, accounts, bladeUtils, bladeNavigationService, dialogService, uiGridHelper) {
                $scope.uiGridConstants = uiGridHelper.uiGridConstants;
                var blade = $scope.blade;
                blade.title = "platform.blades.sessions-list.title";

                blade.refresh = function () {
                    blade.isLoading = true;

                    accounts.sessions({
                        userId: blade.userId,
                        keyword: filter.keyword,
                        skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                        take: $scope.pageSettings.itemsPerPageCount
                    }, function (data) {
                        blade.isLoading = false;

                        $scope.pageSettings.totalItems = data.totalCount;
                        blade.currentEntities = data.results;
                    });
                };

                $scope.terminate = function (session) {
                    var dialog = {
                        id: "confirmTerminateSession",
                        title: "platform.dialogs.session-terminate.title",
                        message: "platform.dialogs.session-terminate.message",
                        callback: function (remove) {
                            if (remove) {
                                accounts.terminateSession({ id: session.id }, null, () => {
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
                    }
                ];


                var filter = $scope.filter = {};
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
