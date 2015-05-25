angular.module('platformWebApp')
.controller('platformWebApp.settingDictionaryController', ['$scope', 'platformWebApp.dialogService', function ($scope, dialogService) {

    $scope.dictValueValidator = function (value) {
    	if ($scope.blade.currentEntity.valueType == 'ShortText') {
            return _.all(currentEntities, function (item) { return angular.lowercase(item.value) !== angular.lowercase(value); });
        } else {
            return _.all(currentEntities, function (item) { return item.value !== value; });
        }
    }

    $scope.add = function (form) {
        if (form.$valid) {
            currentEntities.push($scope.newValue);
            resetNewValue();
            form.$setPristine();
        }
    };

    $scope.delete = function (index) {
        currentEntities.splice(index, 1);
        $scope.selectedItem = undefined;
    };

    $scope.selectItem = function (listItem) {
        $scope.selectedItem = listItem;
    };

    $scope.bladeHeadIco = 'fa fa-wrench';
    $scope.bladeToolbarCommands = [
     {
         name: "Delete", icon: 'fa fa-trash-o',
         executeMethod: function () {
             deleteChecked();
         },
         canExecuteMethod: function () {
             return isItemsChecked();
         }
     }
    ];

    $scope.checkAll = function (selected) {
        angular.forEach(currentEntities, function (item) {
            item._selected = selected;
        });
    };

    function resetNewValue() {
        $scope.newValue = { value: null };
    }

    function isItemsChecked() {
        return _.any(currentEntities, function (x) { return x._selected; });
    }

    function deleteChecked() {
        var dialog = {
            id: "confirmDeleteItem",
            title: "Delete confirmation",
            message: "Are you sure you want to delete selected Values?",
            callback: function (remove) {
                if (remove) {
                    var selection = _.where(currentEntities, { _selected: true });
                    angular.forEach(selection, function (listItem) {
                        $scope.delete(currentEntities.indexOf(listItem));
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.$watch('blade.parentBlade.currentEntities', function (data) {
        var allEntities = _.flatten(_.map(data, _.values));
        $scope.blade.currentEntity = _.findWhere(allEntities, { name: $scope.blade.currentEntityId });
        currentEntities = $scope.blade.currentEntity.arrayValues;
    });

    // on load
    var currentEntities;
    resetNewValue();
    $scope.blade.isLoading = false;
}]);