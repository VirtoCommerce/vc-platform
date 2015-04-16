angular.module('platformWebApp')
.controller('accountRolesController', ['$scope', 'bladeNavigationService', 'settings', 'dialogService', function ($scope, bladeNavigationService, settings, dialogService) {
    
    function initializeBlade(data) {
        $scope.blade.promise.then(function (promiseData) {
            _.each(promiseData.roles, function (x) {
                x.isChecked = _.some(data.roles, function (curr) { return curr.id === x.id; });
            });
            
            $scope.blade.currentEntities = angular.copy(promiseData.roles);
            $scope.blade.origEntity = promiseData.roles;
            $scope.blade.isLoading = false;
        });
    };
    
    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The Roles has been modified. Do you want to save changes?"
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

    function isDirty() {
        return !angular.equals($scope.blade.currentEntities, $scope.blade.origEntity);
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.isValid = function () {
        return true;
    }

    $scope.saveChanges = function () {
        $scope.blade.data.roles = _.where($scope.blade.currentEntities, { isChecked: true });
        
        angular.copy($scope.blade.currentEntities, $scope.blade.origEntity);
        $scope.bladeClose();
    };

    $scope.bladeHeadIco = 'fa-lock';
    
    //var row = _.findWhere($scope.gridOptions.data, { id: role.id });

    //function assignRole(role, isAdd) {
    //    if (isAdd) {
    //        if (!_.findWhere($scope.blade.currentEntity.roles, { id: role.id })) {
    //            $scope.blade.currentEntity.roles.push(role);
    //        }
    //    } else {
    //        var idx = _.findIndex($scope.blade.currentEntity.roles, function (x) { return x.id === role.id; });
    //        if (idx >= 0) {
    //            $scope.blade.currentEntity.roles.splice(idx, 1);
    //        }
    //    }
    //};

    $scope.$watch('blade.parentBlade.currentEntity', function (currentEntity) {
        $scope.blade.data = currentEntity;
        initializeBlade($scope.blade.data);
    });

    // on load: 
    // $scope.$watch('blade.parentBlade.currentEntity' gets fired
}]);