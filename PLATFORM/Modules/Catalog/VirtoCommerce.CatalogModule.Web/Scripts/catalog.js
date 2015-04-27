//Call this to register our module to main application
var catalogsModuleName = "virtoCommerce.catalogModule";

if (AppDependencies != undefined) {
    AppDependencies.push(catalogsModuleName);
}

angular.module(catalogsModuleName, [
  'textAngular'
])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.catalog', {
              url: '/catalog',
              templateUrl: 'Modules/$(VirtoCommerce.Catalog)/Scripts/home/home.tpl.html',
              controller: [
                  '$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'categories',
                          title: 'Catalogs',
                          breadcrumbs: [],
                          subtitle: 'Manage catalogs',
                          controller: 'virtoCommerce.catalogModule.catalogsListController',
                          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalogs-list.tpl.html',
                          isClosingDisabled: true
                      };
                      bladeNavigationService.showBlade(blade);
                  }
              ]
          });
  }
  ]
)
.run(
  ['$rootScope', 'mainMenuService', 'widgetService', '$state', 'notificationTemplateResolver', 'bladeNavigationService', function ($rootScope, mainMenuService, widgetService, $state, notificationTemplateResolver, bladeNavigationService) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/catalog',
          icon: 'fa fa-folder',
          title: 'Catalog',
          priority: 20,
          action: function () { $state.go('workspace.catalog') },
          permission: 'catalogMenuPermission'
      };
      mainMenuService.addMenuItem(menuItem);

      //NOTIFICATIONS
      var menuImportTemplate =
		 {
		     priority: 900,
		     satisfy: function (notify, place) { return place == 'menu' && notify.notifyType == 'ImportNotifyEvent'; },
		     template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/notifications/menuImport.tpl.html',
		     action: function (notify) { $state.go('notificationsHistory', notify) }
		 };
      notificationTemplateResolver.register(menuImportTemplate);

      var historyImportTemplate =
		{
		    priority: 900,
		    satisfy: function (notify, place) { return place == 'history' && notify.notifyType == 'ImportNotifyEvent'; },
		    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/notifications/historyImport.tpl.html',
		    action: function (notify) {
		        var blade = {
		            id: 'CatalogImportDetail',
		            title: 'catalog import detail',
		            subtitle: 'detail',
		            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/import/import-job-progress.tpl.html',
		            controller: 'virtoCommerce.catalogModule.importJobProgressController',
		            job: notify.job
		        };
		        bladeNavigationService.showBlade(blade);
		    }
		};
      notificationTemplateResolver.register(historyImportTemplate);

      //Register image widget
      var itemImageWidget = {
          controller: 'virtoCommerce.catalogModule.itemImageWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/widgets/itemImageWidget.tpl.html',
      };
      widgetService.registerWidget(itemImageWidget, 'itemDetail');
      //Register item property widget
      var itemPropertyWidget = {
          controller: 'virtoCommerce.catalogModule.itemPropertyWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/widgets/itemPropertyWidget.tpl.html',
      };
      widgetService.registerWidget(itemPropertyWidget, 'itemDetail');

      //Register item associations widget
      var itemAssociationsWidget = {
          controller: 'virtoCommerce.catalogModule.itemAssociationsWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/widgets/itemAssociationsWidget.tpl.html',
      };
      widgetService.registerWidget(itemAssociationsWidget, 'itemDetail');

      //Register item seo widget
      var itemSeoWidget = {
          controller: 'virtoCommerce.catalogModule.seoWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/widgets/seoWidget.tpl.html',
      };
      widgetService.registerWidget(itemSeoWidget, 'itemDetail');

      //Register item editorialReview widget
      var editorialReviewWidget = {
          controller: 'virtoCommerce.catalogModule.editorialReviewWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/widgets/editorialReviewWidget.tpl.html',
      };
      widgetService.registerWidget(editorialReviewWidget, 'itemDetail');

      //Register variation widget
      var variationWidget = {
          controller: 'virtoCommerce.catalogModule.itemVariationWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/widgets/itemVariationWidget.tpl.html',
      };
      widgetService.registerWidget(variationWidget, 'itemDetail');
      //Register asset widget
      var itemAssetWidget = {
          controller: 'virtoCommerce.catalogModule.itemAssetWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/widgets/itemAssetWidget.tpl.html',
      };
      widgetService.registerWidget(itemAssetWidget, 'itemDetail');

      //Register category property widget
      var categoryPropertyWidget = {
          controller: 'virtoCommerce.catalogModule.categoryPropertyWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/widgets/categoryPropertyWidget.tpl.html',
      };

      widgetService.registerWidget(categoryPropertyWidget, 'categoryDetail');



      //Register category seo widget
      var categorySeoWidget = {
          controller: 'virtoCommerce.catalogModule.seoWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/widgets/seoWidget.tpl.html',
      };

      widgetService.registerWidget(categorySeoWidget, 'categoryDetail');


      var catalogLanguagesWidget = {
          controller: 'virtoCommerce.catalogModule.catalogLanguagesWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/widgets/catalogLanguagesWidget.tpl.html',
      };
      widgetService.registerWidget(catalogLanguagesWidget, 'catalogDetail');

      var catalogPropertyWidget = {
          controller: 'virtoCommerce.catalogModule.catalogPropertyWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/widgets/catalogPropertyWidget.tpl.html',
      };
      widgetService.registerWidget(catalogPropertyWidget, 'catalogDetail');

  }]);
