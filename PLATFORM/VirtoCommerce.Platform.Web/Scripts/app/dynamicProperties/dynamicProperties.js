angular.module('platformWebApp')
.config(['$stateProvider', function ($stateProvider) {
    $stateProvider
        .state('workspace.dynamicProperties', {
            url: '/dynamicProperties',
            templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
            controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                var blade = {
                    id: 'dynamicPropertiesTypes',
                    controller: 'platformWebApp.dynamicObjectListController',
                    template: '$(Platform)/Scripts/app/dynamicProperties/blades/dynamicObject-list.tpl.html',
                    isClosingDisabled: true
                };
                bladeNavigationService.showBlade(blade);
            }
            ]
        });
}]
)
.run(
  ['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
      var menuItem = {
          path: 'configuration/dynamicProperties',
          icon: 'fa fa-pencil-square-o',
          title: 'Dynamic properties',
          priority: 2,
          action: function () { $state.go('workspace.dynamicProperties'); },
          permission: 'platform:dynamic_properties:access'
      };
      mainMenuService.addMenuItem(menuItem);
  }])
.filter('dynamicPropertyValueTypeToText', function () {
    return function (input) {
        var retVal;
        switch (input) {
            case 'ShortText': retVal = 'Short text'; break;
            case 'LongText': retVal = 'Long text'; break;
            case 'Integer': retVal = 'Integer'; break;
            case 'Decimal': retVal = 'Decimal'; break;
            case 'DateTime': retVal = 'Date'; break;
            case 'Boolean': retVal = 'Boolean'; break;
            case 'Html': retVal = 'HTML'; break;
            default:
                retVal = input ? input : 'Undefined';
        }
        return retVal;
    }
});