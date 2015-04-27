//Call this to register our module to main application
var moduleName = "virtoCommerce.marketingModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.marketingModule', {
              url: '/marketing',
              templateUrl: 'Modules/$(VirtoCommerce.Core)/Scripts/home.tpl.html',
              controller: [
                  '$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'marketing',
                          title: 'Marketing',
                          controller: 'virtoCommerce.marketingModule.marketingMainController',
                          template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/common/marketing-main.tpl.html',
                          isClosingDisabled: true
                      };
                      bladeNavigationService.showBlade(blade);
                  }
              ]
          });
  }]
)
.run(
  ['$rootScope', 'mainMenuService', 'widgetService', '$state', 'virtoCommerce.coreModule.common.dynamicExpressionService', function ($rootScope, mainMenuService, widgetService, $state, dynamicExpressionService) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/marketing',
          icon: 'fa fa-flag',
          title: 'Marketing',
          priority: 40,
          action: function () { $state.go('workspace.marketingModule'); },
          permission: 'marketingMenuPermission'
      };
      mainMenuService.addMenuItem(menuItem);

      //Register expressions
      dynamicExpressionService.registerExpression({
          id: 'BlockCustomerCondition',
          newChildLabel: '+ add user group',
          getValidationError: function () {
              return (this.children && this.children.length) ? undefined : 'Promotion requires at least one eligibility';
          }
      });
      dynamicExpressionService.registerExpression({
          id: 'ConditionIsEveryone',
          displayName: 'Everyone'
      });
      dynamicExpressionService.registerExpression({
          id: 'ConditionIsFirstTimeBuyer',
          displayName: 'First time buyer'
      });
      dynamicExpressionService.registerExpression({
          id: 'ConditionIsRegisteredUser',
          displayName: 'Registered user'
      });

      var availableExcludings = [
            {
                id: 'ExcludingCategoryCondition'
            },
            {
                id: 'ExcludingProductCondition'
            }
      ];
      dynamicExpressionService.registerExpression({
          id: 'ExcludingCategoryCondition',
          displayName: 'Items of category'
      });
      dynamicExpressionService.registerExpression({
          id: 'ExcludingProductCondition',
          displayName: 'Items of entry'
      });

      dynamicExpressionService.registerExpression({
          id: 'BlockCatalogCondition',
          newChildLabel: '+ add condition'
      });
      dynamicExpressionService.registerExpression({
          id: 'ConditionEntryIs',
          // templateURL: 'expression-ConditionEntryIs.html',
          // controller: 'conditionEntryIsController'
          displayName: 'Product is []'
      });
      dynamicExpressionService.registerExpression({
          id: 'ConditionCurrencyIs',
          displayName: 'Currency is []'
      });
      dynamicExpressionService.registerExpression({
          id: 'ConditionCodeContains',
          displayName: 'Product code contains []'
      });
      dynamicExpressionService.registerExpression({
          id: 'ConditionCategoryIs',
          displayName: 'Category is []'
      });

      dynamicExpressionService.registerExpression({
          id: 'BlockCartCondition',
          newChildLabel: '+ add condition'
      });
      dynamicExpressionService.registerExpression({
          id: 'ConditionCartSubtotalLeast',
          displayName: 'Cart subtotal is []',
          availableChildren: availableExcludings,
          newChildLabel: '+ excluding'
      });
      dynamicExpressionService.registerExpression({
          id: 'ConditionAtNumItemsInCart',
          displayName: '[] [] items are in shopping cart',
          availableChildren: availableExcludings,
          newChildLabel: '+ excluding'
      });
      dynamicExpressionService.registerExpression({
          id: 'ConditionAtNumItemsInCategoryAreInCart',
          displayName: '[] [] items of category are in shopping cart',
          availableChildren: availableExcludings,
          newChildLabel: '+ excluding'
      });
      dynamicExpressionService.registerExpression({
          id: 'ConditionAtNumItemsOfEntryAreInCart',
          displayName: '[] [] items of entry are in shopping cart'
      });

      dynamicExpressionService.registerExpression({
          id: 'RewardBlock',
          newChildLabel: '+ add effect',
          getValidationError: function () {
              return (this.children && this.children.length) ? undefined : 'Promotion requires at least one reward';
          }
      });
      dynamicExpressionService.registerExpression({
          id: 'RewardCartGetOfAbsSubtotal',
          displayName: 'Get $[] off cart subtotal'
      });
      dynamicExpressionService.registerExpression({
          id: 'RewardCartGetOfRelSubtotal',
          displayName: 'Get [] % off cart subtotal'
      });
      dynamicExpressionService.registerExpression({
          id: 'RewardItemGetFreeNumItemOfProduct',
          displayName: 'Get [] free items of product []'
      });
      dynamicExpressionService.registerExpression({
          id: 'RewardItemGiftNumItem',
          displayName: 'Gift [] of product []'
      });
      dynamicExpressionService.registerExpression({
          id: 'RewardItemGetOfAbs',
          displayName: 'Get $[] off'
      });
      dynamicExpressionService.registerExpression({
          id: 'RewardItemGetOfRel',
          displayName: 'Get [] % off'
      });
      dynamicExpressionService.registerExpression({
          id: 'RewardItemGetOfAbsForNum',
          displayName: 'Get $[] off [] items of entry []'
          //availableChildren: availableExcludings,
          //newChildLabel: '+ excluding'
      });
      dynamicExpressionService.registerExpression({
          id: 'RewardItemGetOfRelForNum',
          displayName: 'Get [] % off [] items of entry []'
          //availableChildren: availableExcludings,
          //newChildLabel: '+ excluding'
      });
      dynamicExpressionService.registerExpression({
          id: 'RewardShippingGetOfAbsShippingMethod',
          displayName: 'Get $[] off shipping []'
      });
      dynamicExpressionService.registerExpression({
          id: 'RewardShippingGetOfRelShippingMethod',
          displayName: 'Get [] % off shipping []'
      });
  }]);