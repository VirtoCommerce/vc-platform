angular.module('virtoCommerce.catalogModule')
.controller('propertyDictionaryController', ['$scope', '$filter', 'dialogService', function ($scope, $filter, dialogService) {
    var pb = $scope.blade.parentBlade;
    $scope.pb = pb;

    $scope.dictValueValidator = function (value) {
    	return _.all(pb.currentEntity.dictionaryValues, function (item) { return item.value !== value; });
    }


    $scope.add = function (form) {
    	if (form.$valid) {
        	pb.currentEntity.dictionaryValues.push($scope.newValue);
            resetNewValue($scope.newValue.languageCode);
            form.$setPristine();
        }
    };

    $scope.delete = function (index) {
    	pb.currentEntity.dictionaryValues.splice(index, 1);
        $scope.selectedItem = undefined;
    };

    $scope.selectItem = function (listItem) {
        $scope.selectedItem = listItem;
    };

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
    	angular.forEach(pb.currentEntity.dictionaryValues, function (item) {
            item.selected = selected;
        });
    };

    function resetNewValue(locale) {
        $scope.newValue = { languageCode: locale, value: null, propertyId: pb.currentEntity.id };
    }

    function isItemsChecked() {
        return _.any(pb.currentEntity.dictionaryValues, function (x) { return x.selected; });
    }

    function deleteChecked() {
        var dialog = {
            id: "confirmDeleteItem",
            title: "Delete confirmation",
            message: "Are you sure you want to delete selected dictionaryValues?",
            callback: function (remove) {
                if (remove) {
                    var selection = $filter('filter')(pb.currentEntity.dictionaryValues, { selected: true }, true);
                    angular.forEach(selection, function (listItem) {
                        $scope.delete(pb.currentEntity.dictionaryValues.indexOf(listItem));
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    // on load
    resetNewValue(pb.currentEntity.catalog.defaultLanguage.languageCode);
    $scope.blade.isLoading = false;
}]);
