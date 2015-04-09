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
                          controller: 'marketingMainController',
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
  ['$rootScope', 'mainMenuService', 'widgetService', '$state', 'vaDynamicExpressionService', function ($rootScope, mainMenuService, widgetService, $state, vaDynamicExpressionService) {
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
      vaDynamicExpressionService.registerExpression({
          id: 'BlockCustomerCondition',
          newChildLabel: '+ add user group',
          getValidationError: function () {
              return (this.children && this.children.length) ? undefined : 'Promotion requires at least one eligibility';
          }
      });
      vaDynamicExpressionService.registerExpression({
          id: 'ConditionIsEveryone',
          displayName: 'Everyone'
      });
      vaDynamicExpressionService.registerExpression({
          id: 'ConditionIsFirstTimeBuyer',
          displayName: 'First time buyer'
      });
      vaDynamicExpressionService.registerExpression({
          id: 'ConditionIsRegisteredUser',
          displayName: 'Registered user'
      });

      vaDynamicExpressionService.registerExpression({
          id: 'BlockCatalogCondition',
          newChildLabel: '+ add condition'
      });
      vaDynamicExpressionService.registerExpression({
          id: 'ConditionEntryIs',
          // templateURL: 'expression-ConditionEntryIs.html',
          // controller: 'conditionEntryIsController'
          displayName: 'Product is []'
      });
      vaDynamicExpressionService.registerExpression({
          id: 'ConditionCurrencyIs',
          displayName: 'Currency is []'
      });
      vaDynamicExpressionService.registerExpression({
          id: 'ConditionCodeContains',
          displayName: 'Product code contains []'
      });
      vaDynamicExpressionService.registerExpression({
          id: 'ConditionCategoryIs',
          displayName: 'Category is []'
      });

      vaDynamicExpressionService.registerExpression({
          id: 'BlockCartCondition',
          newChildLabel: '+ add condition'
      });

      vaDynamicExpressionService.registerExpression({
          id: 'RewardBlock',
          newChildLabel: '+ add effect',
          getValidationError: function () {
              return (this.children && this.children.length) ? undefined : 'Promotion requires at least one reward';
          }
      });
      vaDynamicExpressionService.registerExpression({
          id: 'RewardCartGetOfAbsSubtotal',
          displayName: 'Get $ [] off cart subtotal'
      });
      vaDynamicExpressionService.registerExpression({
          id: 'RewardCartGetOfRelSubtotal',
          displayName: 'Get [] % off cart subtotal'
      });

  }]);