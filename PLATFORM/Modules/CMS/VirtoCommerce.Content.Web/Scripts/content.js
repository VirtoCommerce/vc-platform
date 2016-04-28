//Call this to register our module to main application
var moduleName = "virtoCommerce.contentModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.run(['platformWebApp.authService', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', 'platformWebApp.bladeNavigationService', 'platformWebApp.permissionScopeResolver', 'virtoCommerce.storeModule.stores', 'virtoCommerce.contentModule.menuLinkList-associationTypesService',
	function (authService, mainMenuService, widgetService, $state, bladeNavigationService, scopeResolver, stores, associationTypesService) {

	    var menuItem = {
	        path: 'browse/content',
	        icon: 'fa fa-code',
	        title: 'content.main-menu-title',
	        priority: 111,
	        action: function () { $state.go('workspace.content'); },
	        permission: 'content:access'
	    };
	    mainMenuService.addMenuItem(menuItem);

	    //Register widgets
	    //widgetService.registerWidget({
	    //    controller: 'virtoCommerce.contentModule.mainContentItemWidgetController',
	    //    size: [6, 2],
	    //    template: 'Modules/$(VirtoCommerce.Content)/Scripts/widgets/mainContentItemWidget.tpl.html',
	    //}, 'contentMainListItem');

	    //widgetService.registerWidget({
	    //    controller: 'virtoCommerce.contentModule.storeWidgetController',
	    //    size: [2, 2],
	    //    template: 'Modules/$(VirtoCommerce.Content)/Scripts/widgets/storeWidget.tpl.html',
	    //}, 'contentMainListItem');

	    //widgetService.registerWidget({
	    //    controller: 'virtoCommerce.contentModule.themeActiveWidgetController',
	    //    size: [4, 1],
	    //    template: 'Modules/$(VirtoCommerce.Content)/Scripts/widgets/themeActiveWidget.tpl.html',
	    //}, 'contentMainListItem');

	    //widgetService.registerWidget({
	    //    controller: 'virtoCommerce.contentModule.themesWidgetController',
	    //    template: 'Modules/$(VirtoCommerce.Content)/Scripts/widgets/themesWidget.tpl.html',
	    //}, 'contentMainListItem');

	    //widgetService.registerWidget({
	    //    controller: 'virtoCommerce.contentModule.pagesWidgetController',
	    //    template: 'Modules/$(VirtoCommerce.Content)/Scripts/widgets/pagesWidget.tpl.html',
	    //}, 'contentMainListItem');

	    //widgetService.registerWidget({
	    //    controller: 'virtoCommerce.contentModule.linkListsWidgetController',
	    //    template: 'Modules/$(VirtoCommerce.Content)/Scripts/widgets/linkListsWidget.tpl.html',
	    //}, 'contentMainListItem');

	    //widgetService.registerWidget({
	    //    controller: 'virtoCommerce.contentModule.blogsWidgetController',
	    //    template: 'Modules/$(VirtoCommerce.Content)/Scripts/widgets/blogsWidget.tpl.html',
	    //}, 'contentMainListItem');

	    // themes widget in STORE details
	    widgetService.registerWidget({
	        size: [2, 1],
	        controller: 'virtoCommerce.contentModule.storeCMSWidgetController',
	        template: 'Modules/$(VirtoCommerce.Content)/Scripts/widgets/storeCMSWidget.tpl.html',
	        permission: 'content:read'
	    }, 'storeDetail');

	    //Register permission scopes templates used for scope bounded definition in role management ui
	    var selectedStoreScope = {
	        type: 'ContentSelectedStoreScope',
	        title: 'Only for selected stores',
	        selectFn: function (blade, callback) {
	            var newBlade = {
	                id: 'store-pick',
	                title: 'content.blades.scope-value-pick-from-simple-list.title',
	                subtitle: 'content.blades.scope-value-pick-from-simple-list.subtitle',
	                currentEntity: this,
	                onChangesConfirmedFn: callback,
	                dataPromise: stores.query().$promise,
	                controller: 'platformWebApp.security.scopeValuePickFromSimpleListController',
	                template: '$(Platform)/Scripts/app/security/blades/common/scope-value-pick-from-simple-list.tpl.html'
	            };
	            bladeNavigationService.showBlade(newBlade, blade);
	        }
	    };
	    scopeResolver.register(selectedStoreScope);

	    // register available types of links in linked lists
	    function openItemSelectWizard(parentElement, blade) {
	        var selectedListEntries = [];
	        var newBlade = {
	            id: "CatalogEntrySelect",
	            title: "content.blades.catalog-items-select.title-product",
	            controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
	            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
	            breadcrumbs: [],
	            toolbarCommands: [
                {
                    name: "platform.commands.pick-selected", icon: 'fa fa-plus',
                    executeMethod: function (blade) {
                        parentElement.associatedObjectId = selectedListEntries[0].id;
                        parentElement.associatedObjectName = selectedListEntries[0].name;
                        bladeNavigationService.closeBlade(blade);
                    },
                    canExecuteMethod: function () {
                        return selectedListEntries.length == 1;
                    }
                }]
	        };

	        newBlade.options = {
	            showCheckingMultiple: false,
	            checkItemFn: function (listItem, isSelected) {
	                if (listItem.type == 'category') {
	                    newBlade.error = 'Must select Product';
	                    listItem.selected = undefined;
	                } else {
	                    if (isSelected) {
	                        if (_.all(selectedListEntries, function (x) { return x.id != listItem.id; })) {
	                            selectedListEntries.push(listItem);
	                        }
	                    }
	                    else {
	                        selectedListEntries = _.reject(selectedListEntries, function (x) { return x.id == listItem.id; });
	                    }
	                    newBlade.error = undefined;
	                }
	            }
	        };

	        bladeNavigationService.showBlade(newBlade, blade);
	    }

	    function openCategorySelectWizard(parentElement, blade) {
	        if (!authService.checkPermission('content:update')) {
	            return;
	        }

	        var selectedListEntries = [];
	        var newBlade = {
	            id: "CatalogCategorySelect",
	            title: "content.blades.catalog-items-select.title-category",
	            controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
	            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
	            breadcrumbs: [],
	            toolbarCommands: [
                {
                    name: "platform.commands.pick-selected", icon: 'fa fa-plus',
                    executeMethod: function (blade) {
                        parentElement.associatedObjectId = selectedListEntries[0].id;
                        parentElement.associatedObjectName = selectedListEntries[0].name;
                        bladeNavigationService.closeBlade(blade);
                    },
                    canExecuteMethod: function () {
                        return selectedListEntries.length == 1;
                    }
                }]
	        };

	        newBlade.options = {
	            showCheckingMultiple: false,
	            allowCheckingItem: false,
	            allowCheckingCategory: true,
	            checkItemFn: function (listItem, isSelected) {
	                if (listItem.type != 'category') {
	                    newBlade.error = 'Must select Category';
	                    listItem.selected = undefined;
	                } else {
	                    if (isSelected) {
	                        if (_.all(selectedListEntries, function (x) { return x.id != listItem.id; })) {
	                            selectedListEntries.push(listItem);
	                        }
	                    }
	                    else {
	                        selectedListEntries = _.reject(selectedListEntries, function (x) { return x.id == listItem.id; });
	                    }
	                    newBlade.error = undefined;
	                }
	            }
	        };

	        bladeNavigationService.showBlade(newBlade, blade);
	    }

	    associationTypesService.registerType({ id: 'product', name: 'Product', openSelectWizard: openItemSelectWizard });
	    associationTypesService.registerType({ id: 'category', name: 'Category', openSelectWizard: openCategorySelectWizard });
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
				        title: 'content.blades.content-main.title',
				        subtitle: 'content.blades.content-main.subtitle',
				        controller: 'virtoCommerce.contentModule.contentMainController',
				        template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/content-main.tpl.html',
				        isClosingDisabled: true
				    };
				    bladeNavigationService.showBlade(blade);
				}
		    ]
		});
}])
    // service for managing association Types for menuLinkList items (links)
.factory('virtoCommerce.contentModule.menuLinkList-associationTypesService', [function () {
    return {
        objects: [],
        registerType: function (entry) {
            if (!entry.templateURL) {
                entry.templateURL = 'linkListSelectObject.tpl';
            }

            this.objects.push(entry);
        }
    };
}]);
