angular.module('virtoCommerce.coreModule.settings.blades', [
   'virtoCommerce.coreModule.settings.resources.setting'
])
.controller('settingsListController', ['$injector', '$scope', 'settings', 'bladeNavigationService',
function ($injector, $scope, settings, bladeNavigationService) {
    var selectedNode = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        settings.getModules({}, function (results) {
            $scope.blade.isLoading = false;

            $scope.objects = results;

            if (selectedNode != null) {
                //select the node in the new list
                angular.forEach(results, function (node) {
                    if (selectedNode.id === node.id) {
                        selectedNode = node;
                    }
                });
            }
        });
    };

    $scope.selectNode = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.id;

        var newBlade = {
            id: 'settingsSection',
            title: selectedNode.title + ' settings',
            moduleId: selectedNode.id,
            controller: 'settingsDetailController',
            template: 'Modules/Core/VirtoCommerce.Core.Web/Scripts/Settings/blades/settings-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };


    // actions on load
    $scope.blade.refresh();
}]);