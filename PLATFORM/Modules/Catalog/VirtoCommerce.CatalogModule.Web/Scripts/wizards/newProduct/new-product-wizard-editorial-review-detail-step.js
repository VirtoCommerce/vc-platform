angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.editorialReviewDetailWizardStepController', ['$scope', function ($scope)
{
    function initializeBlade(data)
    {
        $scope.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    $scope.saveChanges = function ()
    {
        if (angular.isUndefined($scope.blade.parentBlade.currentEntities))
        {
            //If there is no list of reviews save directly to item reviews
            if (angular.isUndefined($scope.blade.parentBlade.item.reviews)) {
                $scope.blade.parentBlade.item.reviews = [];
            }
            $scope.blade.parentBlade.item.reviews.push($scope.currentEntity);
        } else {
            var idx = $scope.blade.parentBlade.currentEntities.indexOf($scope.blade.origEntity);
            $scope.blade.parentBlade.currentEntities.splice(idx, idx < 0 ? 0 : 1, $scope.currentEntity);
        }

        $scope.bladeClose();
    };

    $scope.bladeToolbarCommands = [
        {
            name: "Delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                if (angular.isDefined($scope.blade.parentBlade.currentEntities))
                {
                    var idx = $scope.blade.parentBlade.currentEntities.indexOf($scope.blade.origEntity);
                    $scope.blade.parentBlade.currentEntities.splice(idx, 1);
                    $scope.bladeClose();
                }
            },
            canExecuteMethod: function ()
            {
                if (angular.isDefined($scope.blade.parentBlade.currentEntities)) {
                    return $scope.blade.parentBlade.currentEntities.indexOf($scope.blade.origEntity) >= 0;
                }
                return false;
            }
        }
    ];

    // on load
    initializeBlade($scope.blade.currentEntity);

}]);
