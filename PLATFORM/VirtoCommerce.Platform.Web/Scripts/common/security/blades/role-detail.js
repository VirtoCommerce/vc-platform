angular.module('platformWebApp')
.controller('roleDetailController', ['$scope', 'bladeNavigationService', 'platform_res_roles', 'dialogService', function ($scope, bladeNavigationService, roles, dialogService) {
    var promise = roles.queryPermissions().$promise;

    $scope.blade.refresh = function (parentRefresh) {
        if ($scope.blade.isNew) {
            initializeBlade({});
        } else {
            roles.get({ id: $scope.blade.data.id }, function (data) {
                initializeBlade(data);
                if (parentRefresh) {
                    $scope.blade.parentBlade.refresh();
                }
            });
        }
    }

    function initializeBlade(data) {
        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;

        if ($scope.blade.isNew) {
            promise.then(function (promiseData) {
                $scope.blade.isLoading = false;
                $scope.blade.currentEntities = promiseData;
            });
        } else {
            $scope.blade.isLoading = false;
        }
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    $scope.saveChanges = function () {
        $scope.blade.isLoading = true;

        if ($scope.blade.isNew) {
            $scope.blade.currentEntity.permissions = _.where($scope.blade.currentEntities, { isChecked: true });
        }

        angular.copy($scope.blade.currentEntity, $scope.blade.origEntity);

        roles.update({}, $scope.blade.currentEntity, function (data) {
            if ($scope.blade.isNew) {
                $scope.blade.parentBlade.refresh();
                $scope.blade.parentBlade.selectNode(data);
            } else {
                $scope.blade.refresh(true);
            }
            $scope.blade.refresh(true);
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "The Role has been modified. Do you want to save changes?"
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
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.bladeHeadIco = 'fa-lock';

    function initializeToolbar() {
        if (!$scope.blade.isNew) {
            $scope.bladeToolbarCommands = [
                {
                    name: "Save",
                    icon: 'fa fa-save',
                    executeMethod: function () {
                        $scope.saveChanges();
                    },
                    canExecuteMethod: function () {
                        return isDirty() && $scope.formScope && $scope.formScope.$valid;
                    }
                },
                {
                    name: "Reset",
                    icon: 'fa fa-undo',
                    executeMethod: function () {
                        angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
                    },
                    canExecuteMethod: function () {
                        return isDirty();
                    }
                },
                {
                    name: "Manage permissions", icon: 'fa fa-edit',
                    executeMethod: function () {
                        var newBlade = {
                            id: 'listItemChildChild',
                            promise: promise,
                            title: $scope.blade.title,
                            subtitle: 'Manage permissions',
                            controller: 'rolePermissionsController',
                            template: 'Scripts/common/security/blades/role-permissions.tpl.html'
                        };

                        bladeNavigationService.showBlade(newBlade, $scope.blade);
                    },
                    canExecuteMethod: function () {
                        return true;
                    }
                }
            ];
        }
    }

    

    // actions on load
    initializeToolbar();
    $scope.blade.refresh(false);
}]);