angular.module('platformWebApp')
.controller('platformWebApp.accountListController', ['$scope', 'platformWebApp.accounts', 'platformWebApp.dialogService', 'platformWebApp.uiGridHelper', 
                                                     'platformWebApp.bladeNavigationService', 'platformWebApp.bladeUtils', 'platformWebApp.settings', 
                                                     'platformWebApp.roles', 'platformWebApp.clipboardService',
function ($scope, accounts, dialogService, uiGridHelper, bladeNavigationService, bladeUtils, settings, roles, clipboardService) {
    $scope.uiGridConstants = uiGridHelper.uiGridConstants;
    var blade = $scope.blade;

    // --- Filter state (must be initialized before blade.refresh) ---

    var filter = $scope.filter = {
        keyword: '',
        onlyLocked: false,
        emailNotConfirmed: false,
        userType: '',
        status: '',
        role: '',
        datePreset: '',
        loginStartDate: null,
        loginEndDate: null,

        hasActiveFilters: function () {
            return filter.onlyLocked ||
                   filter.emailNotConfirmed ||
                   filter.userType ||
                   filter.status ||
                   filter.role ||
                   filter.datePreset;
        },

        clearFilters: function () {
            filter.onlyLocked = false;
            filter.emailNotConfirmed = false;
            filter.userType = '';
            filter.status = '';
            filter.role = '';
            filter.datePreset = '';
            filter.loginStartDate = null;
            filter.loginEndDate = null;
            filter.criteriaChanged();
        },

        criteriaChanged: function () {
            computeDateRange();
            if ($scope.pageSettings.currentPage > 1) {
                $scope.pageSettings.currentPage = 1;
            } else {
                blade.refresh();
            }
        }
    };

    function computeDateRange() {
        if (filter.datePreset === 'custom') {
            return;
        }
        var now = new Date();
        var startOfToday = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        filter.loginStartDate = null;
        filter.loginEndDate = null;

        switch (filter.datePreset) {
            case 'today':
                filter.loginStartDate = startOfToday;
                break;
            case 'yesterday':
                filter.loginStartDate = new Date(startOfToday.getTime() - 86400000);
                filter.loginEndDate = startOfToday;
                break;
            case 'last7':
                filter.loginStartDate = new Date(startOfToday.getTime() - 7 * 86400000);
                break;
            case 'last30':
                filter.loginStartDate = new Date(startOfToday.getTime() - 30 * 86400000);
                break;
            default:
                break;
        }
    }

    blade.searchText = '';
    $scope.$watch('blade.searchText', function (newVal, oldVal) {
        if (newVal !== oldVal) {
            filter.keyword = newVal;
            filter.criteriaChanged();
        }
    });

    // --- Blade operations ---

    blade.refresh = function () {
        blade.isLoading = true;

        var searchCriteria = {
            keyword: filter.keyword,
            sort: uiGridHelper.getSortExpression($scope),
            skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
            take: $scope.pageSettings.itemsPerPageCount
        };

        if (filter.onlyLocked) {
            searchCriteria.onlyLocked = true;
        }
        if (filter.emailNotConfirmed) {
            searchCriteria.emailConfirmed = false;
        }
        if (filter.userType) {
            searchCriteria.userType = filter.userType;
        }
        if (filter.status) {
            searchCriteria.status = filter.status;
        }
        if (filter.role) {
            searchCriteria.roles = [filter.role];
        }
        if (filter.loginStartDate) {
            searchCriteria.loginStartDate = filter.loginStartDate;
        }
        if (filter.loginEndDate) {
            searchCriteria.loginEndDate = filter.loginEndDate;
        }

        accounts.search(searchCriteria, function (data) {
            blade.isLoading = false;

            $scope.pageSettings.totalItems = data.totalCount;
            blade.currentEntities = data.results;
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
        });
    };

    blade.selectNode = function (node) {
        $scope.selectedNodeId = node.id;

        var newBlade = {
            id: 'listItemChild',
            data: node,
            title: node.userName,
            subtitle: blade.subtitle,
            controller: 'platformWebApp.accountDetailController',
            template: '$(Platform)/Scripts/app/security/blades/account-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.deleteList = function (selection) {
        var dialog = {
            id: "confirmDeleteItem",
            title: "platform.dialogs.account-delete.title",
            items: [
                { key: 'platform.dialogs.account-delete.account', count: selection.length }
            ],
            callback: function (remove) {
                if (remove) {
                    bladeNavigationService.closeChildrenBlades(blade, function () {
                        var itemIds = _.pluck(selection, 'userName');
                        accounts.remove({ names: itemIds }, blade.refresh);
                    });
                }
            }
        };
        dialogService.showDeleteConfirmationDialog(dialog);
    };

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
            name: "platform.commands.add", icon: 'fas fa-plus',
            executeMethod: function () {
                bladeNavigationService.closeChildrenBlades(blade, function () {
                    var newBlade = {
                        id: 'listItemChild',
                        currentEntity: { roles: [], userType: 'Customer' },
                        title: 'platform.blades.account-detail.title-new',
                        subtitle: blade.subtitle,
                        controller: 'platformWebApp.newAccountWizardController',
                        template: '$(Platform)/Scripts/app/security/wizards/newAccount/new-account-wizard.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                });
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'platform:security:create'
        },
        {
            name: "platform.commands.delete", icon: 'fas fa-trash-alt',
            executeMethod: function () { $scope.deleteList($scope.gridApi.selection.getSelectedRows()); },
            canExecuteMethod: function () {
                return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
            },
            permission: 'platform:security:delete'
        }
    ];

    // --- Load filter options ---

    blade.accountTypes = [];
    blade.accountStatuses = [];
    blade.roles = [];

    settings.get({ id: 'VirtoCommerce.Platform.Security.AccountTypes' }, function (setting) {
        blade.accountTypes = setting.allowedValues || [];
    });
    settings.get({ id: 'VirtoCommerce.Other.AccountStatuses' }, function (setting) {
        blade.accountStatuses = setting.allowedValues || [];
    });
    roles.search({ take: 1000 }, function (data) {
        blade.roles = data.results || [];
    }, function (error) {
        console.error('Failed to load roles:', error);
    });

    blade.datePresets = [
        { label: 'platform.blades.account-list.filter.date-any', value: '' },
        { label: 'platform.blades.account-list.filter.date-today', value: 'today' },
        { label: 'platform.blades.account-list.filter.date-yesterday', value: 'yesterday' },
        { label: 'platform.blades.account-list.filter.date-last7', value: 'last7' },
        { label: 'platform.blades.account-list.filter.date-last30', value: 'last30' },
        { label: 'platform.blades.account-list.filter.date-custom', value: 'custom' }
    ];

    $scope.copy = function (text) {
        clipboardService.copyText(text);
    };

    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
            uiGridHelper.bindRefreshOnSortChanged($scope);
        });
        bladeUtils.initializePagination($scope);
    };

    // actions on load
    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //blade.refresh();
}]);
