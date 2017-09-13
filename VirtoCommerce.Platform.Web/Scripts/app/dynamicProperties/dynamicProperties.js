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
          title: 'platform.menu.dynamic-properties',
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
            case 'ShortText': retVal = 'platform.properties.short-text.title'; break;
            case 'LongText': retVal = 'platform.properties.long-text.title'; break;
            case 'Integer': retVal = 'platform.properties.integer.title'; break;
            case 'Decimal': retVal = 'platform.properties.decimal.title'; break;
            case 'DateTime': retVal = 'platform.properties.date-time.title'; break;
            case 'Boolean': retVal = 'platform.properties.boolean.title'; break;
            case 'Html': retVal = 'platform.properties.html.title'; break;
            default:
                retVal = input ? input : 'platform.properties.undefined.title';
        }
        return retVal;
    }
})
//Filter for showing localized display name in current user language
.filter('localizeDynamicPropertyName', function () {
    return function (input, lang) {
        var retVal = input.name;
        var displayName = _.find(input.displayNames, function (obj) { return obj && obj.locale.startsWith(lang); });
        if (displayName && displayName.name)
            retVal += ' (' + displayName.name + ')';

        return retVal;
    }
});