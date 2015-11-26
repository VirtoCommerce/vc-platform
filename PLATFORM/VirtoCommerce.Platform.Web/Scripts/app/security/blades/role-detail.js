﻿angular.module('platformWebApp')
.controller('platformWebApp.roleDetailController', ['$q', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.roles', 'platformWebApp.dialogService', function ($q, $scope, bladeNavigationService, roles, dialogService) {
    var blade = $scope.blade;
    var promise = roles.queryPermissions().$promise;

    blade.refresh = function (parentRefresh) {
        if (blade.isNew) {
            initializeBlade({});
        } else {            
            roles.get({ id: blade.data.id }, function (data) {
                initializeBlade(data);
                if (parentRefresh && blade.parentBlade.refresh) {
                    blade.parentBlade.refresh();
                }
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        }
    }

    function initializeBlade(data) {
        blade.selectedAll = false;
        blade.currentEntity = angular.copy(data);
        blade.origEntity = data;

        if (blade.isNew) {
            promise.then(function (promiseData) {
                blade.isLoading = false;
                blade.currentEntities = _.groupBy(promiseData, 'groupName');
            });
        } else {
            blade.isLoading = false;
        }        
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity);
    };

    $scope.saveChanges = function () {
        blade.isLoading = true;

        if (blade.isNew) {
            var values = _.flatten(_.values(blade.currentEntities), true);
            blade.currentEntity.permissions = _.where(values, { isChecked: true });
        }

        angular.copy(blade.currentEntity, blade.origEntity);

        roles.update(blade.currentEntity, function (data) {
            if (blade.isNew) {
                blade.parentBlade.refresh();
                blade.parentBlade.selectNode(data);
            } else {
                blade.refresh(true);
            }
            blade.refresh(true);
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
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
        closeChildrenBlades();
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "platform.dialogs.role-save.title",
                message: "platform.dialogs.role-save.message"
            };
            dialog.callback = function (needSave) {
                if (needSave) {
                    $scope.saveChanges();
                }
                closeCallback();
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    function closeChildrenBlades() {
        angular.forEach(blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

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
                    executeMethod: function () {
                        $scope.saveChanges();
                    },
                    canExecuteMethod: function () {
                        return isDirty() && $scope.formScope && $scope.formScope.$valid;
                    },
                    permission: 'platform:security:update'
                },
                {
                    name: "platform.commands.reset",
                    icon: 'fa fa-undo',
                    executeMethod: function () {
                        angular.copy(blade.origEntity, blade.currentEntity);
                    },
                    canExecuteMethod: function () {
                        return isDirty();
                    },
                    permission: 'platform:security:update'
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
                    permission: 'platform:security:update'
                },
                {
                    name: "platform.commands.remove", icon: 'fa fa-trash-o',
                    executeMethod: function () {
                        deleteChecked();
                    },
                    canExecuteMethod: function () {
                        return isItemsChecked();
                    },
                    permission: 'platform:security:update'
                }
            ];
        }
    }



    // actions on load
    initializeToolbar();
    blade.refresh(false);
}]);