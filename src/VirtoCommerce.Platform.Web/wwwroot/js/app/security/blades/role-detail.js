angular.module('platformWebApp').controller('platformWebApp.roleDetailController', ['$q', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.roles', 'platformWebApp.dialogService', function ($q, $scope, bladeNavigationService, roles, dialogService) {
    var blade = $scope.blade;
    blade.updatePermission = 'platform:security:update';
    var promise = roles.queryPermissions().$promise;

    blade.refresh = function (parentRefresh) {
        if (blade.isNew) {
            initializeBlade({});
        } else {
            roles.get({ roleName: blade.data.name }, function (role) {
                initializeBlade(role);
                if (parentRefresh && blade.parentBlade.refresh) {
                    blade.parentBlade.refresh();
                }
            });
        }
    }

    function initializeBlade(role) {
        blade.selectedAll = false;
        blade.currentEntity = angular.copy(role);
        blade.origEntity = role;

        if (blade.isNew) {
            promise.then(function (promiseData) {
                blade.isLoading = false;
                blade.currentEntities = _.groupBy(promiseData, 'groupName');
            });
        } else {
            blade.isLoading = false;
        }
    }

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    }

    function canSave() {
        return isDirty() && $scope.formScope && $scope.formScope.$valid;
    }

    $scope.saveChanges = function () {
        blade.isLoading = true;

        if (blade.isNew) {
            var values = _.flatten(_.values(blade.currentEntities), true);
            blade.currentEntity.permissions = _.where(values, { isChecked: true });
        }

        angular.copy(blade.currentEntity, blade.origEntity);
        var action = blade.isNew ? roles.create : roles.update;
        action(blade.currentEntity, function (result) {
            if (result.succeeded) {
                if (blade.isNew) {
                    blade.parentBlade.refresh();
                    blade.parentBlade.selectNode(blade.currentEntity);
                    blade.data = {};
                }
                blade.data.name = blade.currentEntity.name;
                blade.refresh(true);
            }
            else {
                bladeNavigationService.setError(result.errors.join(), blade);
            }
        });
    };

    $scope.selectNode = function (node) {
        if (_.any(node.availableScopes)) {
            $scope.selectedNodeId = node.id;
            var newBlade = {
                id: 'permissionScopes',
                permission: node,
                title: node.name,
                subtitle: 'platform.blades.permission-scopes.subtitle',
                controller: 'platformWebApp.permissionScopesController',
                template: '$(Platform)/Scripts/app/security/blades/permission-scopes.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        }
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "platform.dialogs.role-save.title", "platform.dialogs.role-save.message");
    };

    $scope.toggleAll = function () {
        angular.forEach(blade.currentEntity.permissions, function (item) {
            item.$selected = blade.selectedAll;
        });
    };

    function isItemsChecked() {
        return blade.currentEntity && _.any(blade.currentEntity.permissions, function (x) { return x.$selected; });
    }

    function deleteChecked() {
        _.each(blade.currentEntity.permissions.slice(), function (x) {
            if (x.$selected) {
                blade.currentEntity.permissions.splice(blade.currentEntity.permissions.indexOf(x), 1);
            }
        });
        blade.selectedAll = false;
    }

    $scope.delete = function (index) {
        blade.currentEntity.permissions.splice(index, 1);
    };

    blade.headIcon = 'fa-key';

    function initializeToolbar() {
        if (!blade.isNew) {
            blade.toolbarCommands = [
                {
                    name: "platform.commands.save",
                    icon: 'fa fa-save',
                    executeMethod: $scope.saveChanges,
                    canExecuteMethod: canSave,
                    permission: blade.updatePermission
                },
                {
                    name: "platform.commands.reset",
                    icon: 'fa fa-undo',
                    executeMethod: function () {
                        angular.copy(blade.origEntity, blade.currentEntity);
                    },
                    canExecuteMethod: isDirty,
                    permission: blade.updatePermission
                },
                {
                    name: "platform.commands.assign", icon: 'fa fa-plus',
                    executeMethod: function () {
                        var newBlade = {
                            id: 'listItemChildChild',
                            promise: promise,
                            title: blade.title,
                            subtitle: 'platform.blades.role-permissions.subtitle',
                            controller: 'platformWebApp.rolePermissionsController',
                            template: '$(Platform)/Scripts/app/security/blades/role-permissions.tpl.html'
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
        }
    }

    // actions on load
    initializeToolbar();
    blade.refresh(false);
}]);
