angular.module('catalogModule.blades.seoDetail', [])
.controller('seoDetailController', ['$scope', 'categories', 'items', 'dialogService', function ($scope, categories, items, dialogService) {
    $scope.blade.origItem = {};

    $scope.blade.refresh = function (parentRefresh) {
        if (parentRefresh) {
            $scope.blade.isLoading = true;
            $scope.blade.parentBlade.refresh().$promise.then(function (dataz) {
                initializeBlade(dataz);
            });
        } else {
            var data = $scope.blade.parentBlade.item.seoInfos;
            initializeBlade(data);
        }
    }

    function initializeBlade(data) {
        $scope.seoInfos = angular.copy(data);
        $scope.blade.origItem = data;
        $scope.blade.title = $scope.blade.parentBlade.item.code;
        $scope.blade.isLoading = false;
    };

    function saveChanges() {
        $scope.blade.isLoading = true;
        if ($scope.blade.seoUrlKeywordType === 0) {
            categories.update({ id: $scope.blade.parentBlade.item.id, seoInfos: $scope.seoInfos }, function (data, headers) {
                $scope.blade.refresh(true);
            });
        } else if ($scope.blade.seoUrlKeywordType === 1) {
            items.updateitem({ id: $scope.blade.parentBlade.item.id, seoInfos: $scope.seoInfos }, function (data, headers) {
                $scope.blade.refresh(true);
            });
        }
    };

    function isDirty() {
        return !angular.equals($scope.seoInfos, $scope.blade.origItem);
    };

    function closeThisBlade(closeCallback) {
        closeCallback();
    };

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The SEO information has been modified. Do you want to save changes?"
            };
            dialog.callback = function (needSave) {
                if (needSave) {
                    saveChanges();
                }
                closeThisBlade(closeCallback);
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeThisBlade(closeCallback);
        }
    };

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    $scope.bladeToolbarCommands = [
        {
            name: "Save", icon: 'icon-floppy',
            executeMethod: function () {
                saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty() && formScope && formScope.$valid;
            }
        },
        {
            name: "Reset", icon: 'icon-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origItem, $scope.seoInfos);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        }
    ];


    $scope.blade.subtitle = 'SEO information';
    $scope.blade.refresh(false);
}]);
