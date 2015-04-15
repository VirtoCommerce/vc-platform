//Call this to register our module to main application
var moduleName = "unknowVendor.customExpression";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.run(['vaDynamicExpressionService', '$templateCache', function (vaDynamicExpressionService, $templateCache) {
  	//Register expressions
  	vaDynamicExpressionService.registerExpression({
  		id: 'ConditionItemWithTag',
  		templateURL: 'expression-ConditionItemWithTag.html',
  		displayName: 'item with [] tags',
  	});
  	$templateCache.put('expression-ConditionItemWithTag.html', 'Items contains tags	 <input va-number required ng-model="element1.numItem" min="0" step="1">');
  }]);