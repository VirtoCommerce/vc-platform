angular.module('platformWebApp')
.controller('platformWebApp.dynamicPropertyListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dynamicProperties.api', function ($scope, bladeNavigationService, dynamicPropertiesApi) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-plus-square-o';
    blade.title = blade.objectType;
    blade.subtitle = 'platform.blades.dynamicProperty-list.subtitle';

    blade.refresh = function (parentRefresh) {
        dynamicPropertiesApi.query({ id: blade.objectType }, function (results) {
            if (parentRefresh && blade.parentRefresh) {
                blade.parentRefresh(results);
            }

            blade.currentEntities = results;
            blade.isLoading = false;
        }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    $scope.selectNode = function (node) {
        $scope.selectedNodeId = node.id;

        var newBlade = {
            subtitle: 'platform.blades.dynamicProperty-detail.subtitle',
            currentEntity: node
        };
        openDetailsBlade(newBlade);
    };

    function openDetailsBlade(node) {
        var newBlade = {
            id: "dynamicPropertyDetail",
            objectType: blade.objectType,
            controller: 'platformWebApp.dynamicPropertyDetailController',
            template: '$(Platform)/Scripts/app/dynamicProperties/blades/dynamicProperty-detail.tpl.html'
        };
        angular.extend(newBlade, node);

        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.toolbarCommands = [
       //{
       //    name: "Refresh", icon: 'fa fa-refresh',
       //    executeMethod: blade.refresh,
       //    canExecuteMethod: function () {
       //        return true;
       //    }
       //},
       {
           name: "platform.commands.add-new-property", icon: 'fa fa-plus',
           executeMethod: function () {
               $scope.selectedNodeId = undefined;
               var newBlade = {
                   subtitle: 'platform.blades.dynamicProperty-detail.subtitle-new',
                   isNew: true,
                   onChangesConfirmedFn: function (entry) {
                       $scope.selectedNodeId = entry.id;
                   }
               };
               openDetailsBlade(newBlade);
           },
           canExecuteMethod: function () {
               return true;
           },
           permission: 'platform:dynamic_properties:create'
       }
    ];

    blade.refresh();
}]);
