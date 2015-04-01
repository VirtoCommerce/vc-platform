angular.module('virtoCommerce.marketingModule')
.controller('marketingMainController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.selectedNodeId = null;

    function initializeBlade() {
        var entities = [
            { id: '3', name: 'Promotions', entityName: 'promotion' },
            { id: '20', name: 'Dynamic content', entityName: 'dynamicContent' },
            { id: '1', name: 'Content publishing', entityName: 'contentPublishing' }];
        $scope.blade.currentEntities = entities;
        $scope.blade.isLoading = false;

        $scope.blade.openBlade(entities[0]);
    };

    $scope.blade.openBlade = function (data) {
        $scope.selectedNodeId = data.id;

        var newBlade = {
            id: 'marketingDetails',
            title: data.name,
            subtitle: 'Marketing service',
            controller: data.entityName + 'ListController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/blades/' + data.entityName + '-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.bladeHeadIco = 'fa fa-flag';

    initializeBlade();
}]);
