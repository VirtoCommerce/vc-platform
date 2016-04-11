angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.customerModule.members', 'virtoCommerce.customerModule.organizations', 'platformWebApp.dynamicProperties.api', 'virtoCommerce.customerModule.memberTypesResolverService', 'platformWebApp.dialogService', 'virtoCommerce.coreModule.common.countries', function ($scope, bladeNavigationService, members, organizations, dynamicPropertiesApi, memberTypesResolverService, dialogService, countries) {
    var blade = $scope.blade;
    blade.updatePermission = 'customer:update';
    blade.isNew = !blade.currentEntity.id;

    blade.refresh = function (parentRefresh) {
        if (blade.isNew) {
            var newEntity = angular.extend({
                dynamicProperties: [],
                addresses: [],
                phones: [],
                emails: []
            }, blade.currentEntity);

            if (newEntity.memberType === 'Organization') {
                newEntity.parentId = blade.parentBlade.currentEntity.id;
            } else if (newEntity.memberType === 'Contact' || newEntity.memberType === 'Employee') {
                newEntity.organizations = [];
                if (blade.parentBlade.currentEntity.id) {
                    newEntity.organizations.push(blade.parentBlade.currentEntity.id);
                }

                if (newEntity.memberType === 'Employee') {
                    newEntity.isActive = true;
                }
            }

            fillDynamicProperties(newEntity, blade.memberTypeDefinition.fullTypeName);
        } else {
            blade.isLoading = true;

            members.get({ id: blade.currentEntity.id }, initializeBlade,
                function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });

            if (parentRefresh) {
                blade.parentBlade.refresh();
            }
        }
    };

    function fillDynamicProperties(newEntity, typeName) {
        dynamicPropertiesApi.query({ id: typeName }, function (results) {
            _.each(results, function (x) {
                x.displayNames = undefined;
                x.values = [];
            });
            newEntity.dynamicProperties = results;
            initializeBlade(newEntity);
        }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }

    function initializeBlade(data) {
        blade.currentEntity = angular.copy(data);
        blade.origEntity = data;
        blade.isLoading = false;
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
            members.save(blade.currentEntity, function (data) {
                blade.parentBlade.refresh();
                blade.origEntity = blade.currentEntity;
                $scope.bladeClose();
            }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        } else {
            members.update(blade.currentEntity, function (data) {
                blade.refresh(true);
            }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        }
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "customer.dialogs.member-save.title", "customer.dialogs.member-save.message");
    };

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
        }
        ];
    }

    // datepicker
    $scope.datepickers = {
        bd: false
    }
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

    blade.headIcon = blade.memberTypeDefinition.icon;
    if (!blade.isNew) {
        blade.subtitle = blade.memberTypeDefinition.subtitle;
    }

    // on load
    if (blade.currentEntity.memberType === 'Contact' || blade.currentEntity.memberType === 'Employee') {
        $scope.organizations = organizations.query();
        $scope.timeZones = countries.getTimeZones();
    }

    blade.refresh(false);
}]);