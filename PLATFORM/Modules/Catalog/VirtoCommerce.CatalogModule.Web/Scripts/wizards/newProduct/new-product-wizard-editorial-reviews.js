angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.newProductWizardReviewsController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService)
{
    $scope.blade.isLoading = false;
    $scope.wizardBlade = $scope.blade.parentBlade;

    $scope.saveChanges = function()
    {
        $scope.blade.parentBlade.item.reviews = $scope.blade.currentEntities;
        $scope.bladeClose();
    };

    $scope.openBlade = function (data)
    {
        var newBlade = {
            id: 'editorialReviewWizard',
            currentEntity: data,
            languages: $scope.wizardBlade.parentBlade.catalog.languages,
            title: 'Review',
            subtitle: 'Product Review',
            controller: 'virtoCommerce.catalogModule.editorialReviewDetailWizardStepController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/editorialReview-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.blade.onClose = function (closeCallback)
    {

        if ($scope.blade.childrenBlades.length > 0)
        {
            var callback = function ()
            {
                if ($scope.blade.childrenBlades.length == 0)
                {
                    closeCallback();
                };
            };
            angular.forEach($scope.blade.childrenBlades, function (child)
            {
                bladeNavigationService.closeBlade(child, callback);
            });
        }
        else
        {
            closeCallback();
        }
    };


    $scope.bladeToolbarCommands = [
        {
            name: "Add", icon: 'fa fa-plus',
            executeMethod: function ()
            {
                var data = { languageCode: $scope.wizardBlade.parentBlade.catalog.defaultLanguage.languageCode };
                $scope.openBlade(data);
            },
            canExecuteMethod: function ()
            {
                return true;
            }
        }
    ];

}]);
