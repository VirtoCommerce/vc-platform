angular.module('platformWebApp')
.config(['$stateProvider', function ($stateProvider) {
    $stateProvider
        .state('workspace.assets', {
            url: '/assets',
            templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
            controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                var blade = {
                    id: 'assetList',
                    controller: 'platformWebApp.assets.assetListController',
                    template: '$(Platform)/Scripts/app/assets/blades/asset-list.tpl.html',
                    isClosingDisabled: true
                };
                bladeNavigationService.showBlade(blade);
            }]
        });
}])
.run(
  ['platformWebApp.mainMenuService', '$state', function (mainMenuService, $state) {
      var menuItem = {
          path: 'browse/assets',
          icon: 'fa fa-folder-o',
          title: 'Assets',
          priority: 130,
          action: function () { $state.go('workspace.assets'); },
          permission: 'platform:assets:access'
      };
      mainMenuService.addMenuItem(menuItem);
  }]);