angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberListController', ['$scope', 'virtoCommerce.customerModule.members', 'virtoCommerce.customerModule.contacts', 'virtoCommerce.customerModule.organizations', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, members, contacts, organizations, bladeNavigationService, dialogService) {
    //pagination settings
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 20;

    $scope.filter = { searchKeyword: undefined };

    var selectedNode = null;
    var preventOrganizationListingOnce; // prevent from unwanted additional actions after command was activated from context menu

    var blade = $scope.blade;
    blade.title = 'Organizations & Customers';

    blade.refresh = function () {
        blade.isLoading = true;
        members.search(
            {
                organization: blade.currentEntity.id,
                keyword: $scope.filter.searchKeyword,
                start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                count: $scope.pageSettings.itemsPerPageCount
            },
		function (data) {
		    blade.isLoading = false;
		    $scope.pageSettings.totalItems = angular.isDefined(data.totalCount) ? data.totalCount : 0;
		    $scope.listEntries = data.members;
		    $scope.pageSettings.selectedAll = false;

		    if (selectedNode != null) {
		        //select the node in the new list
		        angular.forEach($scope.listEntries, function (node) {
		            if (selectedNode.id === node.id) {
		                selectedNode = node;
		            }
		        });
		    }

		    //Set navigation breadcrumbs
		    setBreadcrumbs();
		}, function (error) {
		    bladeNavigationService.setError('Error ' + error.status, blade);
		});
    }

    //Breadcrumbs
    function setBreadcrumbs() {
        if (blade.breadcrumbs) {
            //Clone array (angular.copy leaves the same reference)
            var breadcrumbs = blade.breadcrumbs.slice(0);

            //prevent duplicate items
            if (_.all(breadcrumbs, function (x) { return x.id !== blade.currentEntity.id; })) {
                var breadCrumb = generateBreadcrumb(blade.currentEntity.id, blade.currentEntity.displayName);
                breadcrumbs.push(breadCrumb);
            }
            blade.breadcrumbs = breadcrumbs;
        } else {
            blade.breadcrumbs = [generateBreadcrumb(null, 'all')];
        }
    }

    function generateBreadcrumb(id, name) {
        return {
            id: id,
            name: name,
            blade: blade,
            navigate: function (breadcrumb) {
                //bladeNavigationService.closeBlade(breadcrumb.blade,
                //function () {
                breadcrumb.blade.disableOpenAnimation = true;
                bladeNavigationService.showBlade(breadcrumb.blade);
                breadcrumb.blade.refresh();
                //});
            }
        }
    }

    $scope.$watch('pageSettings.currentPage', function (newPage) {
        blade.refresh();
    });

    $scope.getMainAddress = function (data) {
        var retVal;
        if (_.some(data.addresses)) {
            var primaries = _.where(data.addresses, { Primary: "Primary" });
            if (_.some(primaries)) {
                retVal = primaries[0];
            } else {
                primaries = _.where(data.addresses, { name: "Primary Address" });
                if (_.some(primaries)) {
                    retVal = primaries[0];
                } else {
                    retVal = data.addresses[0];
                }
            }
        }
        return retVal ? (retVal.line1 + ' ' + retVal.city + ' ' + retVal.countryName) : '';
    }

    $scope.getMainEmail = function (data) {
        var retVal;
        if (_.some(data.emails)) {
            retVal = data.emails[0];
        }
        return retVal;
    }

    $scope.edit = function (listItem) {
        if (listItem.memberType === 'Organization') {
            preventOrganizationListingOnce = true;
            blade.showDetailBlade(listItem, listItem.displayName);
        }
        // else do nothing as customer is opened on selecting it.
    };

    blade.showDetailBlade = function (listItem, title) {
        blade.setSelectedNode(listItem);

        var newBlade = {
            id: "listMemberDetail",
            currentEntityId: listItem.id,
            isOrganization: false,
            title: title,
            subtitle: 'Customer details',
            controller: 'virtoCommerce.customerModule.memberDetailController',
            template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/customer-detail.tpl.html'
        };

        if (listItem.memberType === 'Organization') {
            newBlade.isOrganization = true;
            newBlade.subtitle = 'Organization details';
            newBlade.template = 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/organization-detail.tpl.html';
        }

        bladeNavigationService.showBlade(newBlade, blade);
    };

    function isSingleChecked() {
        return _.where($scope.listEntries, { selected: true }).length == 1;
    }

    function getFirstChecked() {
        return _.findWhere($scope.listEntries, { selected: true });
    }

    $scope.delete = function (data) {
        deleteList([data]);

        preventOrganizationListingOnce = true;
    };

    function isItemsChecked() {
        return $scope.listEntries && _.any($scope.listEntries, function (x) { return x.selected; });
    }

    function deleteList(selection) {
        var dialog = {
            id: "confirmDeleteItem",
            title: "Delete confirmation",
            message: "Are you sure you want to delete selected Organizations or Customers?",
            callback: function (remove) {
                if (remove) {
                    bladeNavigationService.closeChildrenBlades(blade, function () {
                        var organizationIds = _.pluck(_.where(selection, { memberType: 'Organization' }), 'id');
                        var customerIds = _.pluck(_.where(selection, { memberType: 'Contact' }), 'id');

                        if (_.any(organizationIds)) {
                            organizations.remove({ ids: organizationIds },
                            blade.refresh,
                            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                        }
                        if (_.any(customerIds)) {
                            contacts.remove({ ids: customerIds },
                            blade.refresh,
                            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                        }
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    blade.setSelectedNode = function (listItem) {
        selectedNode = listItem;
        $scope.selectedNodeId = selectedNode.id;
    };

    $scope.selectNode = function (listItem) {
        blade.setSelectedNode(listItem);

        if (preventOrganizationListingOnce) {
            preventOrganizationListingOnce = false;
        } else {
            if (listItem.memberType === 'Organization') {
                var newBlade = {
                    id: 'memberList',
                    breadcrumbs: blade.breadcrumbs,
                    subtitle: 'Browsing "' + listItem.displayName + '"',
                    currentEntity: listItem,
                    disableOpenAnimation: true,
                    controller: blade.controller,
                    template: blade.template,
                    isClosingDisabled: true
                };
                bladeNavigationService.showBlade(newBlade, blade.parentBlade);
            } else {
                blade.showDetailBlade(listItem, listItem.displayName);
            }
        }
    };

    blade.headIcon = 'fa-user';

    blade.toolbarCommands = [
        {
            name: "Refresh", icon: 'fa fa-refresh',
            executeMethod: function () {
                blade.refresh();
            },
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "Add", icon: 'fa fa-plus',
            executeMethod: function () {
                var newBlade = {
                    id: 'listItemChild',
                    title: 'New member',
                    subtitle: 'Choose new member type',
                    controller: 'virtoCommerce.customerModule.memberAddController',
                    template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/member-add.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, blade);
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'customer:create'
        },
        {
            name: "Manage", icon: 'fa fa-edit',
            executeMethod: function () {
                var listItem = getFirstChecked();
                blade.showDetailBlade(listItem, listItem.displayName);
            },
            canExecuteMethod: isSingleChecked,
            permission: 'customer:update'
        },
        {
            name: "Delete", icon: 'fa fa-trash-o',
            executeMethod: function () { deleteList(_.where($scope.listEntries, { selected: true })); },
            canExecuteMethod: isItemsChecked,
            permission: 'customer:delete'
        }
    ];

    $scope.checkAll = function (selected) {
        angular.forEach($scope.listEntries, function (item) {
            item.selected = selected;
        });
    };

    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //blade.refresh();
}]);
