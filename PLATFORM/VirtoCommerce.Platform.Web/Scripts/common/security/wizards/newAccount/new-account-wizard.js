angular.module('platformWebApp')
.controller('platformWebApp.newAccountWizardController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.accounts', 'platformWebApp.roles', function ($scope, bladeNavigationService, accounts, roles) {
    var promise = roles.get({ count: 10000 }).$promise;

    function initializeBlade(data) {
        promise.then(function (promiseData) {
            $scope.blade.isLoading = false;
            $scope.blade.currentEntities = promiseData.roles;
        });
    };

    $scope.saveChanges = function () {
        if ($scope.blade.currentEntity.password != $scope.blade.currentEntity.newPassword2) {
            $scope.blade.error = 'Error: passwords don\'t match!';
            return;
        }

        $scope.blade.isLoading = true;
        $scope.blade.error = undefined;
        var postData = angular.copy($scope.blade.currentEntity);
        postData.newPassword2 = undefined;
        postData.roles = _.where($scope.blade.currentEntities, { isChecked: true });

        accounts.save({}, postData, function (data) {
            $scope.blade.parentBlade.refresh();
            $scope.blade.parentBlade.selectNode(data);
        }, function (error) {
            var errText = 'Error ' + error.status;
            if (error.data && error.data.message) {
                errText = errText + ": " + error.data.message;
            }
            bladeNavigationService.setError(errText, $scope.blade);
        });
    };
    
    $scope.bladeHeadIco = 'fa-lock';
    
    // actions on load
    initializeBlade($scope.blade.currentEntity);
}]);