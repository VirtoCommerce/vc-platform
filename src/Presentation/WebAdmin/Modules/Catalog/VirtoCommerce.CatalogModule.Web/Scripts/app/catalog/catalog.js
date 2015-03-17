//Call this to register our module to main application
var catalogsModuleName = "catalogModules";

if (AppDependencies != undefined) {
    AppDependencies.push(catalogsModuleName);
}

angular.module(catalogsModuleName, [
  'catalogModule.blades.catalogsList',
  'catalogModule.blades.catalogAdd',
  'catalogModule.blades.catalogDetail',
  'catalogModule.blades.catalogsSelect',
  'catalogModule.blades.virtualCatalogDetail',
  'catalogModule.blades.catalogPropertyDetail',
  'catalogModule.blades.categoryPropertyDetail',
  'catalogModule.blades.categoryDetail',
  'catalogModule.blades.categoriesItemsList',
  'catalogModule.widget.categoryPropertyWidget',
  'catalogModule.blades.itemDetail',
  'catalogModule.blades.seoDetail',
  'catalogModule.blades.propertyDetail',
  'catalogModule.widget.catalogLanguagesWidget',
   'catalogModule.widget.catalogPropertyWidget',
  'catalogModule.blades.catalogLanguages',
  'catalogModule.widget.seoWidget',
  'catalogModule.directives',
  'catalogModule.wizards.newProductWizard',
  'catalogModule.wizards.newProductWizard.images',
  'catalogModule.wizards.newProductWizard.properties',
  'catalogModule.wizards.newProductWizard.reviews',
  'catalogModule.wizards.newProductWizard.review.detail',
  'catalogModule.wizards.newProductWizard.seo',
  'catalogModule.blades.import.importJobList',
  'catalogModule.blades.import.importJobRun',
  'catalogModule.wizards.importJobWizard',
  'catalogModule.wizards.importJobWizard.catalogs',
  'catalogModule.wizards.importJobWizard.importers',
  'catalogModule.wizards.importJobWizard.settings',
  'catalogModule.wizards.importJobWizard.mapping',
  'catalogModule.wizards.importJobWizard.mapping.edit',
  'catalogModule.blades.import',
  'catalogModule.wizards.exportWizard',
  'textAngular'
])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.catalog', {
              url: '/catalog',
              templateUrl: 'Modules/$(VirtoCommerce.Catalog)/Scripts/app/catalog/home/home.tpl.html',
              controller: [
                  '$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'categories',
                          title: 'Catalogs',
                          breadcrumbs: [],
                          subtitle: 'Manage catalogs',
                          controller: 'catalogsListController',
                          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/app/catalog/blades/catalogs-list.tpl.html',
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
          priority: 90,
          action: function () { $state.go('workspace.catalog') },
          permission: 'catalogMenuPermission'
      };
      mainMenuService.addMenuItem(menuItem);

      //NOTIFICATIONS
      var menuImportTemplate =
		 {
		     priority: 900,
		     satisfy: function (notify, place) { return place == 'menu' && notify.notifyType == 'ImportNotifyEvent'; },
		     template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/app/catalog/notifications/menuImport.tpl.html',
		     action: function (notify) { $state.go('notificationsHistory', notify) }
		 };
      notificationTemplateResolver.register(menuImportTemplate);

      var historyImportTemplate =
		{
		    priority: 900,
		    satisfy: function (notify, place) { return place == 'history' && notify.notifyType == 'ImportNotifyEvent'; },
		    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/app/catalog/notifications/historyImport.tpl.html',
		    action: function (notify) {
		        var blade = {
		            id: 'CatalogImportDetail',
		            title: 'catalog import detail',
		            subtitle: 'detail',
		            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/app/catalog/blades/import/import-job-progress.tpl.html',
		            controller: 'importJobProgressController',
		            job: notify.job
		        };
		        bladeNavigationService.showBlade(blade);
		    }
		};
      notificationTemplateResolver.register(historyImportTemplate);

      //Register image widget
      var itemImageWidget = {
          controller: 'itemImageWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/app/catalog/widgets/itemImageWidget.tpl.html',
      };
      widgetService.registerWidget(itemImageWidget, 'itemDetail');
      //Register item property widget
      var itemPropertyWidget = {
          controller: 'itemPropertyWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/app/catalog/widgets/itemPropertyWidget.tpl.html',
      };
      widgetService.registerWidget(itemPropertyWidget, 'itemDetail');

      //Register item associations widget
      var itemAssociationsWidget = {
          controller: 'itemAssociationsWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/app/catalog/widgets/itemAssociationsWidget.tpl.html',
      };
      widgetService.registerWidget(itemAssociationsWidget, 'itemDetail');

      //Register item seo widget
      var itemSeoWidget = {
          controller: 'seoWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/app/catalog/widgets/seoWidget.tpl.html',
      };
      widgetService.registerWidget(itemSeoWidget, 'itemDetail');

      //Register item editorialReview widget
      var editorialReviewWidget = {
          controller: 'editorialReviewWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/app/catalog/widgets/editorialReviewWidget.tpl.html',
      };
      widgetService.registerWidget(editorialReviewWidget, 'itemDetail');

      //Register variation widget
      var variationWidget = {
          controller: 'itemVariationWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/app/catalog/widgets/itemVariationWidget.tpl.html',
      };
      widgetService.registerWidget(variationWidget, 'itemDetail');
      //Register asset widget
      var itemAssetWidget = {
          controller: 'itemAssetWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/app/catalog/widgets/itemAssetWidget.tpl.html',
      };
      widgetService.registerWidget(itemAssetWidget, 'itemDetail');

      //Register category property widget
      var categoryPropertyWidget = {
          controller: 'categoryPropertyWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/app/catalog/widgets/categoryPropertyWidget.tpl.html',
      };

      widgetService.registerWidget(categoryPropertyWidget, 'categoryDetail');

  

      //Register category seo widget
      var categorySeoWidget = {
          controller: 'seoWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/app/catalog/widgets/seoWidget.tpl.html',
      };

      widgetService.registerWidget(categorySeoWidget, 'categoryDetail');

    
      var catalogLanguagesWidget = {
          controller: 'catalogLanguagesWidgetController',
          template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/app/catalog/widgets/catalogLanguagesWidget.tpl.html',
      };
      widgetService.registerWidget(catalogLanguagesWidget, 'catalogDetail');

	 var catalogPropertyWidget = {
       	controller: 'catalogPropertyWidgetController',
      	template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/app/catalog/widgets/catalogPropertyWidget.tpl.html',
	 };
	 widgetService.registerWidget(catalogPropertyWidget, 'catalogDetail');

  }]);
