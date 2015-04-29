angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.editorialReviewDetailController', ['$scope', '$filter', 'dialogService', 'virtoCommerce.catalogModule.items', function ($scope, $filter, dialogService, items) {
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

    function saveChanges(data) {
        $scope.blade.isLoading = true;
        var entriesCopy = $scope.blade.parentBlade.currentEntities.slice();

        if (angular.isDefined(data.id)) {
            entriesCopy = _.reject(entriesCopy, function (ent) { return ent.id === data.id; });
        }

        entriesCopy.push(data);

        items.updateitem({ id: $scope.blade.parentBlade.currentEntityId, reviews: entriesCopy }, function () {
            angular.copy($scope.currentEntity, $scope.blade.origEntity);
            $scope.blade.parentBlade.refresh(true);
        });
    };

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "The Review has been modified. Do you want to save changes?",
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
            title: "Delete confirmation",
            message: "Are you sure you want to delete this Editorial Review?",
            callback: function (remove) {
                if (remove) {
                    $scope.blade.isLoading = true;

                    var idx = $scope.blade.parentBlade.currentEntities.indexOf($scope.blade.origEntity);
                    if (idx >= 0) {
                        var entriesCopy = $scope.blade.parentBlade.currentEntities.slice();
                        entriesCopy.splice(idx, 1);
                        items.updateitem({ id: $scope.blade.parentBlade.currentEntityId, reviews: entriesCopy }, function () {
                            $scope.bladeClose();
                            $scope.blade.parentBlade.refresh(true);
                        });
                    }
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.bladeToolbarCommands = [
        {
            name: "Save", icon: 'fa fa-save',
            executeMethod: function () {
                saveChanges($scope.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
        {
            name: "Delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteEntry();
            },
            canExecuteMethod: function () {
                return $scope.blade.parentBlade.currentEntities.indexOf($scope.blade.origEntity) >= 0 && !isDirty();
            }
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
