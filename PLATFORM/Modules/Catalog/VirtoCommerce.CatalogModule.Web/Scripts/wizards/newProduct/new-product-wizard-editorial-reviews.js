angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.newProductWizardReviewsController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.blade.isLoading = false;
    $scope.wizardBlade = $scope.blade.parentBlade;

    $scope.saveChanges = function () {
        $scope.blade.parentBlade.item.reviews = $scope.blade.currentEntities;
        $scope.bladeClose();
    };

    $scope.openBlade = function (data) {
        var newBlade = {
            id: 'editorialReviewWizard',
            currentEntity: data,
            languages: $scope.wizardBlade.parentBlade.catalog.languages,
            title: 'catalog.blades.editorialReview-detail.title',
            subtitle: 'catalog.blades.editorialReview-detail.subtitle',
            bottomTemplate: '$(Platform)/Scripts/common/templates/ok.tpl.html',
            controller: 'virtoCommerce.catalogModule.editorialReviewDetailWizardStepController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/editorialReview-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }
    
    $scope.blade.toolbarCommands = [
        {
            name: "platform.commands.add", icon: 'fa fa-plus',
            executeMethod: function () {
                var data = { languageCode: $scope.wizardBlade.parentBlade.catalog.defaultLanguage.languageCode };
                $scope.openBlade(data);
            },
            canExecuteMethod: function () {
                return true;
            }
        }
    ];

}]);
