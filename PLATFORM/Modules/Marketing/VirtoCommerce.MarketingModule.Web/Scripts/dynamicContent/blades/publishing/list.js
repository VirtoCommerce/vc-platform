angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.publishingDynamicContentListController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.search', 'platformWebApp.bladeNavigationService', function ($scope, marketing_dynamicContents_res_search, bladeNavigationService) {
    var blade = $scope.blade;
    blade.currentEntity = undefined;
    blade.currentEntities = [];

    $scope.selectedNodeId = null;

    blade.initialize = function () {
        marketing_dynamicContents_res_search.search({ responseGroup: '8' }, function (data) {
            blade.currentEntities = data.contentPublications;
            blade.isLoading = false;
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    blade.addNewPublishing = function () {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'add_publishing_element',
            title: 'marketing.blades.publishing.publishing-main-step.title-new',
            subtitle: 'marketing.blades.publishing.publishing-main-step.subtitle-new',
            isNew: true,
            controller: 'virtoCommerce.marketingModule.addPublishingFirstStepController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/publishing/publishing-main-step.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    blade.editPublishing = function (data) {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'edit_publishing_element',
            title: 'marketing.blades.publishing.publishing-main-step.title',
            subtitle: 'marketing.blades.publishing.publishing-main-step.subtitle',
            entity: data,
            isNew: false,
            controller: 'virtoCommerce.marketingModule.addPublishingFirstStepController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/publishing/publishing-main-step.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    blade.closeChildrenBlades = function () {
        angular.forEach(blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    blade.publishingClick = function (data) {
        blade.currentEntity = data;
    }

    blade.publishingCheck = function (data) {
        return angular.equals(data, blade.currentEntity);
    }

    $scope.blade.toolbarCommands = [
        {
            name: "platform.commands.add", icon: 'fa fa-plus',
            executeMethod: function () {
                blade.addNewPublishing();
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'marketing:create'
        }
    ];

    $scope.blade.headIcon = 'fa-paperclip';

    blade.initialize();
}]);
