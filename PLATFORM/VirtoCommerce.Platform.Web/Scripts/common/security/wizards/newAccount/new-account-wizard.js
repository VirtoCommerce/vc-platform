angular.module('platformWebApp')
.controller('newAccountWizardController', ['$scope', 'bladeNavigationService', 'accounts', 'dialogService', function ($scope, bladeNavigationService, accounts, dialogService) {

    function initializeBlade(data) {
        $scope.blade.isLoading = false;
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

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.bladeHeadIco = 'fa-lock';

    // actions on load
    initializeBlade($scope.blade.currentEntity);
}]);