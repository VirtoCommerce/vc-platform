angular.module('platformWebApp')
.controller('platformWebApp.dynamicPropertyListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dynamicProperties.api', function ($scope, bladeNavigationService, dynamicPropertiesApi) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-plus-square-o';
    blade.title = blade.objectType;
    blade.subtitle = 'Manage dynamic properties';

    blade.refresh = function () {
        dynamicPropertiesApi.query({ id: blade.objectType }, function (results) {
            blade.currentEntities = results;
            blade.isLoading = false;
        }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    $scope.selectNode = function (node) {
        $scope.selectedNodeId = node.id;

        var newBlade = {
            subtitle: 'Manage property',
            currentEntity: node,
            confirmChangesFn: function (entry) {
                angular.copy(entry, node);
                $scope.saveChanges();
            },
            deleteFn: function () {
                //var idx = blade.currentEntities.indexOf(node);
                //if (idx >= 0) {
                //    blade.currentEntities.splice(idx, 1);
                //}
                dynamicPropertiesApi.delete({ id: blade.objectType, propertyId: node.id },
                    blade.refresh,
                    function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            }
        };
        openDetailsBlade(newBlade);
    }

    $scope.saveChanges = function () {
        dynamicPropertiesApi.save({ id: blade.objectType }, blade.currentEntities,
            blade.refresh,
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    function openDetailsBlade(node) {
        var newBlade = {
            id: "dynamicPropertyDetail",
            controller: 'platformWebApp.dynamicPropertyDetailController',
            template: 'Scripts/app/dynamicProperties/blades/dynamicProperty-detail.tpl.html'
        };
        angular.extend(newBlade, node);

        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.toolbarCommands = [
       //{
       //    name: "Refresh", icon: 'fa fa-refresh',
       //    executeMethod: function () {
       //        blade.refresh();
       //    },
       //    canExecuteMethod: function () {
       //        return true;
       //    }
       //},
       {
           name: "Add new property", icon: 'fa fa-plus',
           executeMethod: function () {
               $scope.selectedNodeId = undefined;
               var newBlade = {
                   subtitle: 'New property',
                   isNew: true,
                   confirmChangesFn: function (entry) {
                       blade.currentEntities.push(entry);
                       $scope.saveChanges();
                   },
               };
               openDetailsBlade(newBlade);
           },
           canExecuteMethod: function () {
               return true;
           },
           permission: 'store:manage'
       }
    ];
    blade.refresh();
}]);
