angular.module('platformWebApp')
    .controller('platformWebApp.securityLoginOnBehalfListController', ['$scope', '$window', '$modal', 'platformWebApp.bladeNavigationService', '$localStorage', 'platformWebApp.validators', 
function ($scope, $window, $modal, bladeNavigationService, $localStorage, validators) {
    var blade = $scope.blade;
    $scope.selectedNodeId = null;
    var nameListLoginOnBehalf = 'loginOnBehalfList-' + blade.currentEntityId;

    blade.refresh = function () {
        if ($localStorage[nameListLoginOnBehalf]) {
            blade.currentEntities = $localStorage[nameListLoginOnBehalf];
        }
        else {
            blade.currentEntities = [];
        }
        
        blade.isLoading = false;
    }

    $scope.selectNode = function (urlLoginOnBehalf) {
        $scope.selectedNodeId = urlLoginOnBehalf;
        openUrlOnBehalf(urlLoginOnBehalf);
    }

    function openUrlOnBehalf(secureUrl) {
        // {store_secure_url}/account/login?UserId={customer_id}
        var url = secureUrl + '/account/impersonate/' + blade.currentEntityId;
        $window.open(url, '_blank');
    }

    function editLoginOnBehalfUrls() {
        var newBlade = {
            id: "editRedirectUris",
            updatePermission: 'platform:security:update',
            data: blade.currentEntities,
            validator: validators.uriWithoutQuery,
            headIcon: 'fa-plus-square-o',
            controller: 'platformWebApp.editArrayController',
            template: '$(Platform)/Scripts/common/blades/edit-array.tpl.html',
            onChangesConfirmedFn: function (values) {
                $localStorage[nameListLoginOnBehalf] = angular.copy(values);
                blade.refresh();
            }
        };

        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.headIcon = 'fa-key';

    blade.toolbarCommands = [
        {
            name: "platform.commands.refresh", icon: 'fa fa-refresh',
            executeMethod: blade.refresh,
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "platform.commands.manage", icon: 'fa fa-edit',
            executeMethod: editLoginOnBehalfUrls,
            canExecuteMethod: function () {
                return true;
            },
            permission: 'platform:security:loginOnBehalf'
        }
    ];

    blade.refresh();
}]);
