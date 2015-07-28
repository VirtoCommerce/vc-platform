angular.module('platformWebApp')
.run(
  ['platformWebApp.toolbarService', 'platformWebApp.bladeNavigationService', function (toolbarService, bladeNavigationService) {
      var toolbarItem = {
          name: "Dynamic properties",
          icon: 'fa fa-plus-square-o',
          index: 10,
          executeMethod: function(blade) {
               var newBlade = {
                   id: 'dynamicObjects',
                   controller: 'platformWebApp.dynamicObjectListController',
                   template: 'Scripts/app/dynamicProperties/blades/dynamicObject-list.tpl.html'
               };
               bladeNavigationService.showBlade(newBlade, blade);
          },
          canExecuteMethod: function () { return true; },
          permission: 'platform:dynamic_properties:manage'
      };
      toolbarService.register(toolbarItem, 'platformWebApp.settingGroupListController');
  }]);