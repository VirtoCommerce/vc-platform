//Call this to register our module to main application
var moduleName = "virtoCommerce.mailingModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
])

.run(
  ['$rootScope', 'mainMenuService', 'widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
     
      //Register widgets in catalog item details
      widgetService.registerWidget({
          controller: 'mailingWidgetController',
          template: 'Modules/$(MailChimp.Mailing)/Scripts/widgets/mailingWidget.tpl.html'
      }, 'moduleDetail');
  }])
;