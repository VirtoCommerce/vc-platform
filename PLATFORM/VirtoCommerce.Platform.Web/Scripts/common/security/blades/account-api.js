angular.module('platformWebApp')
.controller('platformWebApp.accountApiController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.accounts', function ($scope, bladeNavigationService, accounts) {
    
    function initializeBlade(data) {
        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.isValid = function () {
        return true;
    }

    $scope.saveChanges = function () {
        angular.copy($scope.blade.currentEntity, $scope.blade.origEntity);
        $scope.bladeClose();
    };
    
    $scope.bladeHeadIco = 'fa-lock';

    $scope.bladeToolbarCommands = [
        {
            name: "Generate", icon: 'fa fa-refresh',
            executeMethod: function () {
                $scope.generateNewApiAccount("Name", "Hmac");
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'platform:security:manage'
        }
    ];

    $scope.generateNewApiAccount = function (name, type) {
        $scope.blade.isLoading = true;

        accounts.generateNewApiAccount({ "name": name, "type": type }, function (apiAccount) {
            $scope.blade.isLoading = false;
            if ($scope.blade.currentEntity.apiAcounts && $scope.blade.currentEntity.apiAcounts.length > 0) {
                $scope.blade.currentEntity.apiAcounts[0] = apiAccount;
            }
            else {
                $scope.blade.currentEntity.apiAcounts = [apiAccount];
            }
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    };

    $scope.copyCode = function () {
        var secretKey = document.getElementById('secretKey');
        secretKey.focus();
        secretKey.select();
    };
    
    $scope.$watch('blade.parentBlade.currentEntity', initializeBlade);

    // on load: 
    // $scope.$watch('blade.parentBlade.currentEntity' gets fired
}]);