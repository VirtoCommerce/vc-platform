//Call this to register our module to main application
var moduleName = "virtoCommerce.mailingModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
])

.run(
  ['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', 'platformWebApp.authService', function ($rootScope, mainMenuService, widgetService, authService) {
     
      //Register widgets in catalog item details
      widgetService.registerWidget({
          isVisible: function (blade) { return blade.currentEntity.id == 'MailChimp.Mailing' && authService.checkPermission('mailing:manage'); },
          controller: 'virtoCommerce.mailingModule.mailingWidgetController',
          template: 'Modules/$(MailChimp.Mailing)/Scripts/widgets/mailingWidget.tpl.html'
      }, 'moduleDetail');
  }])
;