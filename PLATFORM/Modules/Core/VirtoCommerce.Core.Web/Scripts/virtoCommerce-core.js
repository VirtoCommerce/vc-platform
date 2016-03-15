var moduleName = "virtoCommerce.coreModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
    'virtoCommerce.coreModule.currency',
	'virtoCommerce.coreModule.fulfillment',
	'virtoCommerce.coreModule.seo',
	'virtoCommerce.coreModule.common'
]);