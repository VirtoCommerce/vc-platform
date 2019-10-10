angular.module("platformWebApp")
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.modulesSettings', {
              url: '/settings',
              templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
              controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                  var blade = {
                      id: 'settings',
                      title: 'platform.blades.settingGroup-list.title',
                      //subtitle: 'Manage settings',
                      controller: 'platformWebApp.settingGroupListController',
                      template: '$(Platform)/Scripts/app/settings/blades/settingGroup-list.tpl.html',
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
      //Register module in main menu
      var menuItem = {
          path: 'configuration/settings',
          icon: 'fa fa-gears',
          title: 'platform.menu.settings',
          priority: 1,
          action: function () { $state.go('workspace.modulesSettings'); },
          permission: 'platform:setting:access'
      };
      mainMenuService.addMenuItem(menuItem);
  }])

.factory('platformWebApp.settings.helper', [function () {
    var retVal = {};

    retVal.getSetting = function(settings, settingName) {
        return _.findWhere(settings, { name: settingName });
    };

    retVal.fixValues = function (settings) {
        // parse values as they all are strings
        //var selectedSettings = _.where(settings, { valueType: 'Integer' });
        //_.forEach(selectedSettings, function (setting) {
        //    setting.value = parseInt(setting.value, 10);
        //    if (setting.allowedValues) {
        //        setting.allowedValues = _.map(setting.allowedValues, function (value) { return parseInt(value, 10); });
        //    }
        //});

        //selectedSettings = _.where(settings, { valueType: 'Decimal' });
        //_.forEach(selectedSettings, function (setting) {
        //    setting.value = parseFloat(setting.value);
        //    if (setting.allowedValues) {
        //        setting.allowedValues = _.map(setting.allowedValues, function (value) { return parseFloat(value); });
        //    }
        //});

        //selectedSettings = _.where(settings, { valueType: 'Boolean' });
        //_.forEach(selectedSettings, function (setting) {
        //    setting.value = setting.value && setting.value.toLowerCase() === 'true';
        //    if (setting.allowedValues) {
        //        setting.allowedValues = _.map(setting.allowedValues, function (value) { return value.toLowerCase() === 'true'; });
        //    }
        //});  
    };

    retVal.toApiFormat = function (settings) {
        var selectedSettings = _.where(settings, { isDictionary: true });
        _.forEach(selectedSettings, function (setting) {
            if (setting.allowedValues) {
                setting.allowedValues = _.pluck(setting.allowedValues, 'value');
            }
        });
    };

    return retVal;
}]);

// dictionary Setting values management helper
function DictionarySettingDetailBlade(settingName) {
    this.id = 'dictionarySettingDetails';
    this.currentEntityId = settingName;
    this.isApiSave = true;
    this.controller = 'platformWebApp.settingDictionaryController';
    this.template = '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html';
}
