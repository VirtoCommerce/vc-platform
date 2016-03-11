angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.editorialReviewDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'virtoCommerce.catalogModule.items', 'platformWebApp.settings', function ($scope, bladeNavigationService, dialogService, items, settings) {
    var blade = $scope.blade;
    blade.updatePermission = 'catalog:update';
    var promise = settings.getValues({ id: 'Catalog.EditorialReviewTypes' }).$promise;

    function initializeBlade(data) {
        promise.then(function (promiseData) {
            $scope.types = promiseData;

            if (data.isNew) {
                data.reviewType = $scope.types[0];
            }

            $scope.currentEntity = angular.copy(data);
            blade.origEntity = data;
            blade.isLoading = false;
        });
    };

    function isDirty() {
        return !angular.equals($scope.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    }

    function canSave() {
        return isDirty() && $scope.currentEntity.content;
    }

    function saveChanges() {
        blade.isLoading = true;
        var entriesCopy = _.filter(blade.parentBlade.currentEntities, function (ent) { return !angular.equals(ent, blade.origEntity); });
        entriesCopy.push($scope.currentEntity);

        items.update({ id: blade.parentBlade.currentEntityId, reviews: entriesCopy }, function () {
            angular.copy($scope.currentEntity, blade.origEntity);
            blade.parentBlade.refresh(true);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, saveChanges, closeCallback, "catalog.dialogs.review-save.title", "catalog.dialogs.review-save.message");
    };

    function deleteEntry() {
        var dialog = {
            id: "confirmDelete",
            title: "catalog.dialogs.review-delete.title",
            message: "catalog.dialogs.review-delete.message",
            callback: function (remove) {
                if (remove) {
                    blade.isLoading = true;

                    var idx = blade.parentBlade.currentEntities.indexOf(blade.origEntity);
                    if (idx >= 0) {
                        var entriesCopy = blade.parentBlade.currentEntities.slice();
                        entriesCopy.splice(idx, 1);
                        items.update({ id: blade.parentBlade.currentEntityId, reviews: entriesCopy }, function () {
                            $scope.bladeClose();
                            blade.parentBlade.refresh(true);
                        },
                        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                    }
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    blade.headIcon = 'fa-comments';

    blade.toolbarCommands = [
        {
            name: "platform.commands.save", icon: 'fa fa-save',
            executeMethod: saveChanges,
            canExecuteMethod: canSave,
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, $scope.currentEntity);
            },
            canExecuteMethod: isDirty,
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: deleteEntry,
            canExecuteMethod: function () {
                return blade.parentBlade.currentEntities.indexOf(blade.origEntity) >= 0 && !isDirty();
            },
            permission: blade.updatePermission
        }
    ];

    $scope.openDictionarySettingManagement = function () {
        var newBlade = {
            id: 'settingDetailChild',
            isApiSave: true,
            currentEntityId: 'Catalog.EditorialReviewTypes',
            parentRefresh: function (data) { $scope.types = data; },
            controller: 'platformWebApp.settingDictionaryController',
            template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    // on load
    initializeBlade(blade.currentEntity);
    $scope.$watch('blade.parentBlade.currentEntities', function (newEntities, oldEntities) {
        if (!angular.equals(newEntities, oldEntities)) {
            var currentChild = angular.isDefined($scope.currentEntity.id)
                ? _.find(newEntities, function (ent) { return ent.id === $scope.currentEntity.id; })
                : _.find(newEntities, function (ent) { return ent.content === $scope.currentEntity.content; });

            initializeBlade(currentChild);
        }
    }, true);
}]);
