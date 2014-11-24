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
  'catalogModule.widget.virtualCatalogMappingWidget',
  'catalogModule.blades.categoryPropertyDetail',
  'catalogModule.blades.categoryDetail',
  'catalogModule.blades.categoriesItemsList',
  'catalogModule.widget.categoryPropertyWidget',
  'catalogModule.blades.itemDetail',
  'catalogModule.blades.seoDetail',
  'catalogModule.blades.propertyDetail',
  'catalogModule.widget.catalogLanguagesWidget',
  'catalogModule.blades.catalogLanguages',
  'catalogModule.widget.seoWidget',
  'catalogModule.wizards.newProductWizard',
  'catalogModule.wizards.newProductWizard.images',
  'catalogModule.wizards.newProductWizard.properties',
  'catalogModule.wizards.newProductWizard.reviews',
  'catalogModule.wizards.newProductWizard.review.detail',
  'catalogModule.wizards.newProductWizard.seo',
  'catalogModule.directives',
  'textAngular'
])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.catalog', {
              url: '/catalog',
              templateUrl: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/home/home.tpl.html',
              controller: [
                  '$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'categories',
                          title: 'Catalogs',
                          breadcrumbs: [],
                          subtitle: 'Manage catalogs',
                          controller: 'catalogsListController',
                          template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/catalogs-list.tpl.html',
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
  ['$rootScope', 'mainMenuService', 'widgetService', function ($rootScope, mainMenuService, widgetService) {
      //Register module in main menu
      var menuItem = {
          group: 'Browse',
          icon: 'glyphicon glyphicon-search',
          title: 'Catalogs',
          priority: 90,
          state: 'workspace.catalog',
          permission: 'catalogMenuPermission'
      };
      mainMenuService.addMenuItem(menuItem);

      //Register module widgets
      // mapping widget in virtual catalog
      var mappingWidget = {
          group: 'virtualCatalogDetail',
          controller: 'virtualCatalogMappingWidgetController',
          template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/widgets/virtualCatalogMappingWidget.tpl.html'
      };
      widgetService.registerWidget(mappingWidget);

      //Register image widget
      var itemImageWidget = {
          group: 'itemDetail',
          controller: 'itemImageWidgetController',
          template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/widgets/itemImageWidget.tpl.html',
      };
      widgetService.registerWidget(itemImageWidget);
      //Register item property widget
      var itemPropertyWidget = {
          group: 'itemDetail',
          controller: 'itemPropertyWidgetController',
          template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/widgets/itemPropertyWidget.tpl.html',
      };
      widgetService.registerWidget(itemPropertyWidget);

      //Register item seo widget
      var itemSeoWidget = {
          group: 'itemDetail',
          controller: 'seoWidgetController',
          template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/widgets/seoWidget.tpl.html',
      };
      widgetService.registerWidget(itemSeoWidget);

      //Register item seo widget
      var editorialReviewWidget = {
          group: 'itemDetail',
          controller: 'editorialReviewWidgetController',
          template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/widgets/editorialReviewWidget.tpl.html',
      };
      widgetService.registerWidget(editorialReviewWidget);

      //Register variation widget
      var variationWidget = {
          group: 'itemDetail',
          controller: 'itemVariationWidgetController',
          template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/widgets/itemVariationWidget.tpl.html',
      };
      widgetService.registerWidget(variationWidget);
      //Register asset widget
      var itemAssetWidget = {
          group: 'itemDetail',
          controller: 'itemAssetWidgetController',
          template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/widgets/itemAssetWidget.tpl.html',
      };
      widgetService.registerWidget(itemAssetWidget);

      //Register category property widget
      var categoryPropertyWidget = {
          group: 'categoryDetail',
          controller: 'categoryPropertyWidgetController',
          template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/widgets/categoryPropertyWidget.tpl.html',
      };

      widgetService.registerWidget(categoryPropertyWidget);

      //Register category seo widget
      var categorySeoWidget = {
          group: 'categoryDetail',
          controller: 'seoWidgetController',
          template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/widgets/seoWidget.tpl.html',
      };

      widgetService.registerWidget(categorySeoWidget);

      //Register asset widget
      var catalogLanguagesWidget = {
          group: 'catalogLanguages',
          controller: 'catalogLanguagesWidgetController',
          template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/widgets/catalogLanguagesWidget.tpl.html',
      };
      widgetService.registerWidget(catalogLanguagesWidget);
  }])
.filter('propertydatatype', function () {
    return function (input) {
        var result;
        switch (input) {
            case 0:
                result = "Short text";
                break;
            case 1:
                result = "Long text";
                break;
            case 2:
                result = "Numeric";
                break;
                //case 3:
                //    result = "Date";
                //    break;
            default:
                result = input;
        }
        return result;
    };
})
.filter('propertytype', function () {
    return function (input) {
        var result;
        switch (input) {
            case 0:
                result = "Product";
                break;
            case 1:
                result = "Variation";
                break;
            case 2:
                result = "Category";
                break;
            default:
                result = input;
        }
        return result;
    };
});
