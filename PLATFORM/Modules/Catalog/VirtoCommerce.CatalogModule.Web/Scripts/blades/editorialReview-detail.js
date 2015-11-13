﻿angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.editorialReviewDetailController', ['$scope', '$filter', 'platformWebApp.dialogService', 'virtoCommerce.catalogModule.items', function ($scope, $filter, dialogService, items) {
    $scope.types = ["QuickReview", "FullReview"];

    function initializeBlade(data) {
        if (data.isNew) {
            data.reviewType = $scope.types[0];
        }

        $scope.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals($scope.currentEntity, $scope.blade.origEntity);
    };

    function saveChanges() {
        $scope.blade.isLoading = true;
        var entriesCopy = _.filter($scope.blade.parentBlade.currentEntities, function (ent) { return !angular.equals(ent, $scope.blade.origEntity); });
        entriesCopy.push($scope.currentEntity);

        items.update({ id: $scope.blade.parentBlade.currentEntityId, reviews: entriesCopy }, function () {
            angular.copy($scope.currentEntity, $scope.blade.origEntity);
            $scope.blade.parentBlade.refresh(true);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty() && $scope.currentEntity.content) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "catalog.dialogs.review-save.title",
                message: "catalog.dialogs.review-save.message",
                callback: function (needSave) {
                    if (needSave) {
                        saveChanges();
                    }
                    closeCallback();
                }
            }
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    function deleteEntry() {
        var dialog = {
            id: "confirmDelete",
            title: "catalog.dialogs.review-delete.title",
            message: "catalog.dialogs.review-delete.message",
            callback: function (remove) {
                if (remove) {
                    $scope.blade.isLoading = true;

                    var idx = $scope.blade.parentBlade.currentEntities.indexOf($scope.blade.origEntity);
                    if (idx >= 0) {
                        var entriesCopy = $scope.blade.parentBlade.currentEntities.slice();
                        entriesCopy.splice(idx, 1);
                        items.update({ id: $scope.blade.parentBlade.currentEntityId, reviews: entriesCopy }, function () {
                            $scope.bladeClose();
                            $scope.blade.parentBlade.refresh(true);
                        },
                        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
                    }
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.blade.headIcon = 'fa-comments';

    $scope.blade.toolbarCommands = [
        {
            name: "platform.commands.save", icon: 'fa fa-save',
            executeMethod: function () {
                saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty() && $scope.currentEntity.content;
            },
            permission: 'catalog:update'
        },
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'catalog:update'
        },
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteEntry();
            },
            canExecuteMethod: function () {
                return $scope.blade.parentBlade.currentEntities.indexOf($scope.blade.origEntity) >= 0 && !isDirty();
            },
            permission: 'catalog:update'
        }
    ];

    // on load
    initializeBlade($scope.blade.currentEntity);
    $scope.$watch('blade.parentBlade.currentEntities', function (newEntities, oldEntities) {
        if (!angular.equals(newEntities, oldEntities)) {
            var currentChild = angular.isDefined($scope.currentEntity.id)
                ? _.find(newEntities, function (ent) { return ent.id === $scope.currentEntity.id; })
                : _.find(newEntities, function (ent) { return ent.content === $scope.currentEntity.content; });

            initializeBlade(currentChild);
        }
    }, true);
}]);
