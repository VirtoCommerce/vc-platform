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
              templateUrl: 'Scripts/common/templates/home.tpl.html',
              controller: [
                  '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
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
  ['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', 'platformWebApp.notificationTemplateResolver', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.catalogImportService', 'virtoCommerce.catalogModule.catalogExportService', function ($rootScope, mainMenuService, widgetService, $state, notificationTemplateResolver, bladeNavigationService, catalogImportService, catalogExportService) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/catalog',
          icon: 'fa fa-folder',
          title: 'Catalog',
          priority: 20,
          action: function () { $state.go('workspace.catalog'); },
          permission: 'catalog:query'
      };
      mainMenuService.addMenuItem(menuItem);

      //NOTIFICATIONS
     var menuExportTemplate =
		 {
		 	priority: 900,
		 	satisfy: function (notify, place) { return place == 'menu' && notify.notifyType == 'CatalogExport'; },
		 	template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/export/notifications/menuExport.tpl.html',
		 	action: function (notify) { $state.go('notificationsHistory', notify) }
		 };
     notificationTemplateResolver.register(menuExportTemplate);

      var historyExportTemplate =
		{
		    priority: 900,
		    satisfy: function (notify, place) { return place == 'history' && notify.notifyType == 'CatalogExport'; },
		    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/export/notifications/historyExport.tpl.html',
		    action: function (notify) {
		        var blade = {
		            id: 'CatalogExportDetail',
		            title: 'catalog export detail',
		            subtitle: 'detail',
		            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/export/catalog-CSV-export.tpl.html',
		            controller: 'virtoCommerce.catalogModule.catalogCSVexportController',
		            notification: notify
		        };
		        bladeNavigationService.showBlade(blade);
		    }
		};
      notificationTemplateResolver.register(historyExportTemplate);

      //Register image widget
      var itemImageWidget = {
          controller: 'virtoCommerce.catalogModule.itemImageWidgetController',
          size: [2, 2],
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
          size: [2, 1],
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
          isVisible: function (blade) { return !blade.isNew && blade.controller !== 'virtoCommerce.catalogModule.virtualCatalogDetailController'; },
          controller: 'virtoCommerce.catalogModule.catalogPropertyWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/widgets/catalogPropertyWidget.tpl.html'
      };
      widgetService.registerWidget(catalogPropertyWidget, 'catalogDetail');

      // IMPORT / EXPORT
      catalogImportService.register({});
      catalogExportService.register({
          name: 'VirtoCommerce CSV export',
          description: 'Native VirtoCommerce catalog data export to CSV',
          icon: 'fa fa-file-archive-o',
          controller: 'virtoCommerce.catalogModule.catalogCSVexportController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/export/catalog-CSV-export.tpl.html'
      });
  }]);
