//Call this to register our module to main application
var moduleName = "virtoCommerce.contentModule";

if (AppDependencies != undefined) {
	AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.run(['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', 'platformWebApp.securityRoleScopeService', 'virtoCommerce.contentModule.stores',
	function ($rootScope, mainMenuService, widgetService, $state, securityRoleScopeService, stores) {

		var menuItem = {
			path: 'browse/content',
			icon: 'fa fa-code',
			title: 'Content',
			priority: 111,
			action: function () { $state.go('workspace.content'); },
			permission: 'content:query'
		};
		mainMenuService.addMenuItem(menuItem);

		//Register security scope types used for scope bounded ACL definition
		var getScopesFn = function () {
			return stores.query().$promise.then(function (result) {
				//Scope for each store
				var scopes = _.map(result, function (x) { return "content:store:" + x.id; });
				return scopes;
			});
		};
		securityRoleScopeService.registerScopeGetter(getScopesFn);

		widgetService.registerWidget({
			controller: 'virtoCommerce.contentModule.themesWidgetController',
			template: 'Modules/$(VirtoCommerce.Content)/Scripts/widgets/themesWidget.tpl.html',
			permission: 'content:query'
		}, 'storeDetail');
	}])
.config(['$stateProvider', function ($stateProvider) {
	$stateProvider
		.state('workspace.content', {
			url: '/content?storeId',
			templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
			controller: [
				'$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
					var blade = {
						id: 'content',
						title: 'Content',
						subtitle: 'Content service',
						controller: 'virtoCommerce.contentModule.contentMainController',
						template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/common/content-main.tpl.html',
						isClosingDisabled: true
					};
					bladeNavigationService.showBlade(blade);
				}
			]
		});
}]);
