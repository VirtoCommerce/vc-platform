//Call this to register our module to main application
var moduleName = "virtoCommerce.helpdeskModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
])

.run(
  ['$rootScope', 'mainMenuService', 'widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
     
      //Register widgets in catalog item details
      widgetService.registerWidget({
          isVisible: function (blade) { return blade.currentEntity.id == 'Zendesk.Helpdesk'; },
          controller: 'helpdeskWidgetController',
          template: 'Modules/$(Zendesk.Helpdesk)/Scripts/widgets/helpdeskWidget.tpl.html'
      }, 'moduleDetail');
  }])
;