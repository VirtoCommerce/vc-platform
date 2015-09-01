angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.itemsDynamicContentListController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.search', 'virtoCommerce.marketingModule.dynamicContent.folders', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, marketing_dynamicContents_res_search, marketing_dynamicContents_res_folders, bladeNavigationService, dialogService) {
    var blade = $scope.blade;
    blade.currentEntity = {};

    function refresh() {
        marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: '18' }, function (data) {
            blade.currentEntity.childrenFolders = data.contentFolders;
            blade.currentEntity.items = data.contentItems;
            setBreadcrumbs();
            blade.isLoading = false;
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }

    blade.initializeBlade = function () {
        if (blade.choosenFolder === undefined) {
            blade.choosenFolder = 'ContentItem';
        }
        refresh();
    };

    blade.addNew = function () {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'New content items element',
            subtitle: 'Add new content items element',
            choosenFolder: blade.choosenFolder,
            controller: 'virtoCommerce.marketingModule.addContentItemsElementController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/add.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.addNewFolder = function (data) {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'New content items folder element',
            subtitle: 'Add new content items folder element',
            entity: data,
            isNew: true,
            controller: 'virtoCommerce.marketingModule.addFolderContentItemsController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/folder-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.editFolder = function (data) {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'Edit content items folder element',
            subtitle: 'Edit content items folder element',
            entity: data,
            isNew: false,
            controller: 'virtoCommerce.marketingModule.addFolderContentItemsController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/folder-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.addNewContentItem = function (data) {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'New content item element',
            subtitle: 'Add new content item element',
            entity: data,
            isNew: true,
            controller: 'virtoCommerce.marketingModule.addContentItemsController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/content-item-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.editContentItem = function (data) {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'Edit content item element',
            subtitle: 'Edit content item element',
            entity: data,
            isNew: false,
            controller: 'virtoCommerce.marketingModule.addContentItemsController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/content-item-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.closeChildrenBlades = function () {
        angular.forEach(blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    blade.folderClick = function (contentItemFolder) {
        blade.isLoading = true;
        blade.closeChildrenBlades();

        if (angular.isUndefined(blade.choosenFolder) || !angular.equals(blade.choosenFolder, contentItemFolder.id)) {
            blade.choosenFolder = contentItemFolder.id;
            blade.currentEntity = contentItemFolder;
            refresh();
        }
    }

    function setBreadcrumbs() {
        if (blade.breadcrumbs) {
            var breadcrumbs;
            var index = _.findLastIndex(blade.breadcrumbs, { id: blade.choosenFolder });
            if (index > -1) {
                //Clone array (angular.copy leaves the same reference)
                breadcrumbs = blade.breadcrumbs.slice(0, index + 1);
            }
            else {
                breadcrumbs = blade.breadcrumbs.slice(0);
                breadcrumbs.push(generateBreadcrumb(blade.currentEntity));
            }
            blade.breadcrumbs = breadcrumbs;
        } else {
            blade.breadcrumbs = [(generateBreadcrumb({ id: 'ContentItem', name: 'Items' }))];
        }
    }

    function generateBreadcrumb(node) {
        return {
            id: node.id,
            name: node.name,
            navigate: function () {
                blade.folderClick(node);
            }
        }
    }

    blade.deleteFolder = function (data) {
        var dialog = {
            id: "confirmDeleteContentItemsFolder",
            title: "Delete confirmation",
            message: "Are you sure want to delete content items folder?",
            callback: function (remove) {
                if (remove) {
                    marketing_dynamicContents_res_folders.delete({ ids: [data.id] }, function () {
                        var pathSteps = data.outline.split(';');
                        var id = pathSteps[pathSteps.length - 2];
                        blade.choosenFolder = id;
                        blade.initializeBlade();
                    },
					function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                }
            }
        };

        dialogService.showConfirmationDialog(dialog);
    }

    blade.toolbarCommands = [
		{
		    name: "Add", icon: 'fa fa-plus',
		    executeMethod: function () {
		        blade.addNew();
		    },
		    canExecuteMethod: function () {
		        return true;
		    },
		    permission: 'marketing:manage'
		},
		{
		    name: "Edit folder", icon: 'fa fa-pencil-square-o',
		    executeMethod: function () {
		        blade.editFolder(blade.currentEntity);
		    },
		    canExecuteMethod: function () {
		        return !angular.isUndefined(blade.currentEntity);
		    },
		    permission: 'marketing:manage'
		}
    ];

    blade.headIcon = 'fa-inbox';

    blade.initializeBlade();
}]);
