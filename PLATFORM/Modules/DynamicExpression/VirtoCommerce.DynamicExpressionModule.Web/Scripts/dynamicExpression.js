//Call this to register our module to main application
var moduleName = "virtoCommerce.dynamicExpression";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.run(
  ['$http', '$compile', 'virtoCommerce.coreModule.common.dynamicExpressionService', function ($http, $compile, dynamicExpressionService) {

      //Register PROMOTION expressions
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

      //Register CONTENT PUBLISHING expressions
      var groupNames = ['Browse behavior', 'Shopper profile', 'Shopping cart', 'Geo location'];
      dynamicExpressionService.registerExpression({
          id: 'BlockContentCondition',
          newChildLabel: '+ add condition'
      });
      dynamicExpressionService.registerExpression({
          groupName: groupNames[3],
          id: 'ConditionGeoTimeZone',
          displayName: 'Browsing from a time zone -/+ offset from UTC []'
      });
      dynamicExpressionService.registerExpression({
          groupName: groupNames[3],
          id: 'ConditionGeoZipCode',
          displayName: 'Browsing from zip/postal code []'
      });
      dynamicExpressionService.registerExpression({
          groupName: groupNames[0],
          id: 'ConditionStoreSearchedPhrase',
          displayName: 'Shopper searched for phrase [] in store'
      });

      $http.get('Modules/$(VirtoCommerce.DynamicExpression)/Scripts/all-templates.html').then(function (response) {
          // compile the response, which will put stuff into the cache
          $compile(response.data);
      });
  }]);