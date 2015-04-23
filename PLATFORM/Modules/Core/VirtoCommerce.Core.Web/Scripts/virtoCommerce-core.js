var moduleName = "virtoCommerce.coreModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
	'virtoCommerce.coreModule.fulfillment',
	'virtoCommerce.coreModule.common'
]);