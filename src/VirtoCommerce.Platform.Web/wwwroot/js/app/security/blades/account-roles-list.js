angular.module('platformWebApp')
    .controller('platformWebApp.accountRolesListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, dialogService) {
        var blade = $scope.blade;
        blade.updatePermission = 'platform:security:update';

        function initializeBlade(data) {
            blade.currentEntity = data;
            blade.currentEntities = data.roles;
            blade.isLoading = false;
        }

        blade.selectNode = function (node) {
            $scope.selectedNodeId = node.id;

            var newBlade = {
                id: 'roleDetails',
                data: node,
                title: node.name,
                subtitle: 'platform.blades.role-detail.subtitle',
                controller: 'platformWebApp.roleDetailController',
                template: '$(Platform)/Scripts/app/security/blades/role-detail.tpl.html'
            };

            bladeNavigationService.showBlade(newBlade, blade);
        };

        function isItemsChecked() {
            return _.any(blade.currentEntities, function (x) { return x.$selected; });
        }

        function deleteChecked() {
            //var dialog = {
            //    id: "confirmDeleteItem",
            //    title: "Delete confirmation",
            //    message: "Are you sure you want to delete selected Quote Requests?",
            //    callback: function (remove) {
            //        if (remove) {
            _.each(blade.currentEntities.slice(), function (x) {
                if (x.$selected) {
                    blade.currentEntities.splice(blade.currentEntities.indexOf(x), 1);
                }
            });
            //        }
            //    }
            //}
            //dialogService.showConfirmationDialog(dialog);
        }

        $scope.delete = function (index) {
            blade.currentEntities.splice(index, 1);
        };

        blade.headIcon = 'fa-key';

        blade.toolbarCommands = [
            {
                name: "platform.commands.assign", icon: 'fa fa-plus',
                executeMethod: function () {
                    var newBlade = {
                        id: "accountChildBladeChild",
                        title: blade.title,
                        subtitle: 'platform.blades.account-roles.subtitle',
                        controller: 'platformWebApp.accountRolesController',
                        template: '$(Platform)/Scripts/app/security/blades/account-roles.tpl.html'
                    };

                    bladeNavigationService.showBlade(newBlade, $scope.blade);
                },
                canExecuteMethod: function () {
                    return true;
                },
                permission: blade.updatePermission
            },
            {
                name: "platform.commands.remove", icon: 'fa fa-trash-o',
                executeMethod: function () {
                    deleteChecked();
                },
                canExecuteMethod: function () {
                    return isItemsChecked();
                },
                permission: blade.updatePermission
            }
        ];

        $scope.$watch('blade.parentBlade.currentEntity', initializeBlade);
        $scope.$watch('blade.currentEntity.roles', function (data) {
            _.each(data, function (x) { x.$selected = false });
            blade.currentEntities = data;
        });

        // on load:
        // $scope.$watch('blade.parentBlade.currentEntity' gets fired
    }]);
