angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.placeholdersDynamicContentListController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.search', 'virtoCommerce.marketingModule.dynamicContent.folders', 'platformWebApp.bladeNavigationService', function ($scope, marketing_dynamicContents_res_search, marketing_dynamicContents_res_folders, bladeNavigationService) {
    var blade = $scope.blade;
    blade.currentEntity = {};

    $scope.selectedNodeId = null;

    blade.initialize = function () {
    	if (blade.choosenFolder === undefined) {
    		blade.choosenFolder = 'ContentPlace';
    	}
    	marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: '20' }, function (data) {
    		blade.currentEntity.childrenFolders = data.contentFolders;
    		blade.currentEntity.placeholders = data.contentPlaces;
    		setBreadcrumbs();
    		blade.isLoading = false;
    	},
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
    };

    blade.addNew = function () {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'New placeholders element',
            subtitle: 'Add new placeholders element',
            choosenFolder: blade.choosenFolder,
            controller: 'virtoCommerce.marketingModule.addPlaceholderElementController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/add.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    blade.addNewFolder = function (data) {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'New placeholders folder',
            subtitle: 'Add new placeholders folder',
            entity: data,
            isNew: true,
            controller: 'virtoCommerce.marketingModule.addFolderPlaceholderController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/folder-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    blade.editFolder = function (data) {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'Edit placeholders folder',
            subtitle: 'Edit placeholders folder',
            entity: data,
            isNew: false,
            controller: 'virtoCommerce.marketingModule.addFolderPlaceholderController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/folder-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    blade.addNewPlaceholder = function (data) {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'New placeholders element',
            subtitle: 'Add new placeholders element',
            entity: data,
            isNew: true,
            controller: 'virtoCommerce.marketingModule.addPlaceholderController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/placeholder-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    blade.editPlaceholder = function (data) {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'Edit placeholders element',
            subtitle: 'Edit placeholders element',
            entity: data,
            isNew: false,
            controller: 'virtoCommerce.marketingModule.addPlaceholderController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/placeholder-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    blade.closeChildrenBlades = function () {
        angular.forEach(blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    blade.folderClick = function (placeholderFolder) {
    	blade.isLoading = true;
    	blade.closeChildrenBlades();

    	if (angular.isUndefined(blade.choosenFolder) || !angular.equals(blade.choosenFolder, placeholderFolder.id)) {
    		blade.choosenFolder = placeholderFolder.id;
    		blade.currentEntity = placeholderFolder;
    		marketing_dynamicContents_res_search.search({ folder: placeholderFolder.id, respGroup: '18' }, function (data) {
    			placeholderFolder.childrenFolders = data.contentFolders;
    			placeholderFolder.items = data.contentPlaces;
    			blade.isLoading = false;
    			blade.breadcrumbs.push(
					{
						id: placeholderFolder.id,
						name: placeholderFolder.name,
						blade: blade,
						navigate: function (breadcrumb) {
							bladeNavigationService.closeBlade(blade,
								function () {
									blade.disableOpenAnimation = true;
									bladeNavigationService.showBlade(blade, blade.parentBlade);
									blade.choosenFolder = breadcrumb.id;
								});
						}
					});
    		},
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
    	}
    }

    function setBreadcrumbs() {
    	if (blade.breadcrumbs === undefined) {
    		blade.breadcrumbs = [];
    	}

    	var index = _.findLastIndex(blade.breadcrumbs, { id: blade.choosenFolder });

    	if (index !== -1)
    		blade.breadcrumbs = blade.breadcrumbs.slice(0, index + 1);

    	//catalog breadcrumb by default
    	var breadCrumb = {
    		id: 'ContentPlace',
    		name: 'Places',
    		blade: blade
    	};

    	//prevent duplicate items
    	if (!_.some(blade.breadcrumbs, function (x) { return x.id == breadCrumb.id })) {
    		blade.breadcrumbs.push(breadCrumb);
    	}

    	breadCrumb.navigate = function (breadcrumb) {
    		bladeNavigationService.closeBlade(blade,
				function () {
					blade.disableOpenAnimation = true;
					bladeNavigationService.showBlade(blade, blade.parentBlade);
					blade.choosenFolder = breadcrumb.id;
				});
    	};
    }

    blade.deleteFolder = function (data) {
    	var dialog = {
    		id: "confirmDeleteContentPlaceholdersFolder",
    		title: "Delete confirmation",
    		message: "Are you sure want to delete content placeholders folder?",
    		callback: function (remove) {
    			if (remove) {
    				marketing_dynamicContents_res_folders.delete({ ids: [data.id] }, function () {
    					var pathSteps = data.outline.split(';');
    					var id = pathSteps[pathSteps.length - 2];
    					blade.choosenFolder = id;
    					blade.initialize();
    				},
					function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
    			}
    		}
    	};

    	dialogService.showConfirmationDialog(dialog);
    }

    $scope.blade.toolbarCommands = [
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

    $scope.blade.headIcon = 'fa-location-arrow';

    blade.initialize();
}]);
