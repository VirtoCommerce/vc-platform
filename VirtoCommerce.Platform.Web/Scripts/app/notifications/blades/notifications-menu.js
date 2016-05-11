angular.module('platformWebApp')
.controller('platformWebApp.notificationsMenuController', ['$scope', '$stateParams', 'platformWebApp.bladeNavigationService', function ($scope, $stateParams, bladeNavigationService) {
    var blade = $scope.blade;
    blade.updatePermission = 'platform:notification:read';

    function initializeBlade() {
        var entities = [
            { id: '1', name: 'platform.blades.notifications-list.title', templateName: 'notifications-list', controllerName: 'notificationsListController', icon: 'fa-list', subtitle: 'platform.blades.notifications-list.subtitle' },
            { id: '2', name: 'platform.blades.notifications-journal.title', templateName: 'notifications-journal', controllerName: 'notificationsJournalController', icon: 'fa-book', subtitle: 'platform.blades.notifications-journal.subtitle' }];
        blade.currentEntities = entities;
        blade.isLoading = false;
    };

    blade.openBlade = function (data) {
        if (!blade.hasUpdatePermission()) return;

        $scope.selectedNodeId = data.id;

        var objectId = (angular.isUndefined($stateParams.objectId) || $stateParams.objectId === null) ? 'Platform' : $stateParams.objectId;
        var objectTypeId = (angular.isUndefined($stateParams.objectTypeId) || $stateParams.objectTypeId === null) ? 'Platform' : $stateParams.objectTypeId;
        var newBlade = {
            id: 'marketingMainListChildren',
            title: data.name,
            objectId: objectId,
            objectTypeId: objectTypeId,
            subtitle: data.subtitle,
            controller: 'platformWebApp.' + data.controllerName,
            template: '$(Platform)/Scripts/app/notifications/blades/' + data.templateName + '.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.headIcon = 'fa-envelope';

    initializeBlade();

}]);