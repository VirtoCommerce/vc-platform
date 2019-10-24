angular.module('platformWebApp').controller('platformWebApp.oauthappsListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.oauthapps', function ($scope, bladeNavigationService, dialogService, oauthapps) {
    var blade = $scope.blade;
    blade.updatePermission = 'platform:security:update';
    blade.allSelected = false;

    blade.refresh = function () {
        blade.isLoading = true;

        var criteria = {
            skip: 0,
            take: 99999
        };

        oauthapps.search(criteria, function (data) {
            blade.isLoading = false;
            blade.currentEntities = data.results;
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
        });
    };

    $scope.selectNode = function (node) {
        var newBlade = {
            subtitle: 'platform.blades.oauthapps-detail.title',
            origEntity: node
        };
        openDetailsBlade(newBlade);
    };

    function openDetailsBlade(node) {
        var newBlade = {
            id: "oauthapps-detail",
            title: blade.title,
            controller: 'platformWebApp.oAuthAppsController',
            template: '$(Platform)/Scripts/app/security/blades/oauthapps-detail.tpl.html'
        };

        angular.extend(newBlade, node);
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.headIcon = 'fa-key';

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
                blade.selectedData = undefined;
                var newBlade = {
                    subtitle: 'platform.blades.oauthapps-detail.title-new',
                    isNew: true
                };
                openDetailsBlade(newBlade);
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteChecked();
            },
            canExecuteMethod: function () {
                return isItemsChecked();
            },
            permission: blade.updatePermission
        }
    ];

    function deleteChecked() {
        var dialog = {
            id: "confirmDeleteItem",
            title: "platform.dialogs.account-delete.title",
            message: "platform.dialogs.account-delete.message",
            callback: function (remove) {
                if (remove) {
                    var clientIds = blade.currentEntities.filter(x => x.$selected).map(x => x.clientId);
                    oauthapps.delete({ clientIds }, result => {
                        blade.refresh();
                    });
                }
            }
        };
        dialogService.showConfirmationDialog(dialog);
    }

    function isItemsChecked() {
        return _.any(blade.currentEntities, function (x) { return x.$selected; });
    }

    $scope.toggleAll = function () {
        blade.allSelected = !blade.allSelected;
        blade.currentEntities.forEach(x => x.$selected = blade.allSelected);
    }

    blade.refresh();
}]);
