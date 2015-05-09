angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.propertyAttributesController', ['$scope', '$filter', 'platformWebApp.dialogService', function ($scope, $filter, dialogService) {
    var pb = $scope.blade.parentBlade;
    $scope.pb = pb;

    var formScope;

    $scope.attributeNameValidator = function (value) {
        return _.all(pb.currentEntity.attributes, function (item) { return item.name !== value; });
    }

    $scope.setForm = function (form) {
        formScope = form;
    }

    $scope.add = function () {
        if (formScope.$valid) {
            pb.currentEntity.attributes.push($scope.newValue);
            resetNewValue();
            formScope.$setPristine();
        }
    };

    $scope.delete = function (index) {
        pb.currentEntity.attributes.splice(index, 1);
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
        angular.forEach(pb.currentEntity.attributes, function (item) {
            item.selected = selected;
        });
    };

    function resetNewValue() {
        $scope.newValue = { name: null, value: null };
    }

    function isItemsChecked() {
        return _.any(pb.currentEntity.attributes, function (x) { return x.selected; });
    }

    function deleteChecked() {
        var dialog = {
            id: "confirmDeleteItem",
            title: "Delete confirmation",
            message: "Are you sure you want to delete selected attributes?",
            callback: function (remove) {
                if (remove) {
                    var selection = $filter('filter')(pb.currentEntity.attributes, { selected: true }, true);
                    angular.forEach(selection, function (listItem) {
                        $scope.delete(pb.currentEntity.attributes.indexOf(listItem));
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    // on load
    resetNewValue();
    $scope.blade.isLoading = false;
}]);