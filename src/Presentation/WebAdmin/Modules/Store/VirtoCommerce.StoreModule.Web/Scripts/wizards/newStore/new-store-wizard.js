angular.module('virtoCommerce.storeModule.wizards.newStore', [])
.controller('newStoreWizardController', ['$scope', 'bladeNavigationService', 'stores', 'catalogs', 'dialogService', function ($scope, bladeNavigationService, stores, catalogs, dialogService) {
    $scope.blade.refresh = function (parentRefresh) {
        //stores.get({ id: $scope.blade.currentEntityId }, function(data) {
        //    initializeBlade(data);
        //    if (parentRefresh) {
        //        $scope.blade.parentBlade.refresh();
        //    }
        //});
       
        initializeBlade($scope.blade.currentEntity);
    }

    function initializeBlade(data) {
        $scope.blade.currentEntityId = data.id;
        
        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    //function isDirty() {
    //    var retVal = angular.isDefined($scope.blade.currentEntityId);
    //    return !retVal;
    //};

    $scope.saveChanges = function () {
        $scope.blade.isLoading = true;
        
        stores.save({}, $scope.blade.currentEntity, function (data) {
            $scope.blade.parentBlade.refresh();
            $scope.blade.parentBlade.openBlade(data);
        });
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    //$scope.blade.onClose = function (closeCallback) {
    //    if (isDirty()) {
    //        var dialog = {
    //            id: "confirmCurrentBladeClose",
    //            title: "Save changes",
    //            message: "The Store has been modified. Do you want to save changes?"
    //        };
    //        dialog.callback = function (needSave) {
    //            if (needSave) {
    //                $scope.saveChanges();
    //            }
    //            closeCallback();
    //        };
    //        dialogService.showConfirmationDialog(dialog);
    //    }
    //    else {
    //        closeCallback();
    //    }
    //};
    

    $scope.blade.refresh(false);
    $scope.catalogs = catalogs.getCatalogs();
    $scope.storeStates = [{ id: 'Open', name: 'Open' }, { id: 'Closed', name: 'Closed' }, { id: 'RestrictedAccess', name: 'Restricted Access' }];
}]);