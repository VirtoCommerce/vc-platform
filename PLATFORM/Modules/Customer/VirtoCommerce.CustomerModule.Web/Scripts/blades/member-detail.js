angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.customerModule.contacts', 'virtoCommerce.customerModule.organizations', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, contacts, organizations, dialogService) {
    $scope.blade.currentResource = $scope.blade.isOrganization ? organizations : contacts;

    $scope.blade.refresh = function (parentRefresh) {
        if ($scope.blade.currentEntityId) {
            $scope.blade.isLoading = true;

            $scope.blade.currentResource.get({ _id: $scope.blade.currentEntityId }, function (data) {
                initializeBlade(data);
                if (parentRefresh) {
                    $scope.blade.parentBlade.refresh();
                }
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        } else {
            var newEntity = {
                properties: [],
                addresses: [],
                phones: [],
                emails: []
            };

            if ($scope.blade.isOrganization) {
                newEntity.parentId = $scope.blade.parentBlade.currentEntity.id;
            } else {
                newEntity.organizations = [];
                if ($scope.blade.parentBlade.currentEntity.id) {
                    newEntity.organizations.push($scope.blade.parentBlade.currentEntity.id);
                }
            }

            initializeBlade(newEntity);
        }
    }

    function initializeBlade(data) {
        // temporal workaround
        if (!$scope.blade.isOrganization && data.organizations.length > 0) {
            data.organization = data.organizations[0];
        }

        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    $scope.saveChanges = function () {
        $scope.blade.isLoading = true;

        // temporal workaround
        if (!$scope.blade.isOrganization) {
            $scope.blade.currentEntity.organizations = $scope.blade.currentEntity.organization ? [$scope.blade.currentEntity.organization] : [];
        }

        if ($scope.blade.currentEntityId) {
            $scope.blade.currentResource.update({}, $scope.blade.currentEntity, function (data) {
                $scope.blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, $scope.blade);
            });
        } else {
            $scope.blade.currentResource.save({}, $scope.blade.currentEntity, function (data) {
                $scope.blade.title = data.displayName;
                $scope.blade.currentEntityId = data.id;
                initializeBlade(data);
                $scope.blade.parentBlade.refresh();
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, $scope.blade);
            });
        }
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
                message: "The Customer has been modified. Do you want to save changes?"
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

    $scope.blade.headIcon = $scope.blade.isOrganization ? 'fa fa-university' : 'fa fa-user';

    $scope.blade.toolbarCommands = [
        {
            name: "Save",
            icon: 'fa fa-save',
            executeMethod: function () {
                $scope.saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty() && $scope.formScope && $scope.formScope.$valid;
            },
            permission: 'customer:manage'
        },
        {
            name: "Reset",
            icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'customer:manage'
        }
    ];

    if (!$scope.blade.isOrganization) {
        $scope.blade.toolbarCommands.push(
        {
            name: "Login on behalf",
            icon: 'fa fa-key',
            executeMethod: function () {
                var newBlade = {
                    id: 'memberDetailChild',
                    currentEntityId: $scope.blade.currentEntityId,
                    title: 'Login on behalf of ' + $scope.blade.currentEntity.fullName,
                    controller: 'virtoCommerce.customerModule.loginOnBehalfListController',
                    template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/loginOnBehalf-list.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            },
            canExecuteMethod: function () { return true; },
            permission: 'customer:manage'
        });
    }

    // datepicker
    $scope.datepickers = {
        bd: false
    }

    $scope.showWeeks = true;
    $scope.toggleWeeks = function () {
        $scope.showWeeks = !$scope.showWeeks;
    };

    $scope.clear = function () {
        $scope.blade.currentEntity.birthDate = null;
    };
    $scope.today = new Date();

    $scope.open = function ($event, which) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.datepickers[which] = true;
    };

    $scope.dateOptions = {
        'year-format': "'yyyy'",
        'starting-day': 1
    };

    $scope.formats = ['shortDate', 'dd-MMMM-yyyy', 'yyyy/MM/dd'];
    $scope.format = $scope.formats[0];

    // other on load
    if (!$scope.blade.isOrganization) {
        $scope.organizations = organizations.query();
    }

    $scope.blade.refresh(false);
}]);