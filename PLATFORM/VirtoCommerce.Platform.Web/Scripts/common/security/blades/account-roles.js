angular.module('platformWebApp')
.controller('platformWebApp.accountRolesController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, dialogService) {
    
    function initializeBlade(data) {
        $scope.blade.data = data;
        $scope.blade.promise.then(function (promiseData) {
            _.each(promiseData.roles, function (x) {
                x.isChecked = _.some(data.roles, function (curr) { return curr.id === x.id; });
            });
            
            $scope.blade.currentEntities = promiseData.roles;
            $scope.blade.isLoading = false;
        });
    };
   
    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.isValid = function () {
        return true;
    }

    $scope.saveChanges = function () {
        $scope.blade.data.roles = _.where($scope.blade.currentEntities, { isChecked: true });
        
        $scope.bladeClose();
    };

    $scope.blade.headIcon = 'fa-lock';
  
    $scope.$watch('blade.parentBlade.currentEntity', initializeBlade);

    // on load: 
    // $scope.$watch('blade.parentBlade.currentEntity' gets fired
}]);