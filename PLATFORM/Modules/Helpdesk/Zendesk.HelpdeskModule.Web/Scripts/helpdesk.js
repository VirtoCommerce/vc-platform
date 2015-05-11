//Call this to register our module to main application
var moduleName = "virtoCommerce.helpdeskModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
])

.run(
  ['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', 'platformWebApp.authService', function ($rootScope, mainMenuService, widgetService, authService) {

      widgetService.registerWidget({
          isVisible: function(blade) { return authService.checkPermission('helpdeskModule:manage'); },
          controller: 'virtoCommerce.helpdeskModule.openTicketsWidgetController',
          template: 'Modules/$(Zendesk.Helpdesk)/Scripts/widgets/openTicketsWidget.tpl.html'
      }, 'customerDetail2');

      //Register widgets in catalog item details
      widgetService.registerWidget({
          isVisible: function (blade) { return blade.currentEntity.id == 'Zendesk.Helpdesk'; },
          controller: 'helpdeskWidgetController',
          template: 'Modules/$(Zendesk.Helpdesk)/Scripts/widgets/helpdeskWidget.tpl.html'
      }, 'moduleDetail');
  }])
;