angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberListController', ['$scope', 'virtoCommerce.customerModule.members', 'virtoCommerce.customerModule.contacts', 'virtoCommerce.customerModule.organizations', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, members, contacts, organizations, bladeNavigationService, dialogService) {
    //pagination settings
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 20;

    $scope.filter = { searchKeyword: undefined };

    $scope.pageSettings.selectedAll = false;
    var selectedNode = null;
    var preventOrganizationListingOnce; // prevent from unwanted additional actions after command was activated from context menu
    $scope.blade.title = 'Organizations & Customers';

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        members.search(
            {
                organization: $scope.blade.currentEntity.id,
                keyword: $scope.filter.searchKeyword,
                start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                count: $scope.pageSettings.itemsPerPageCount
            },
		function (data) {
		    $scope.blade.isLoading = false;
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
		    setBreadcrumps();
		}, function (error) {
		    $scope.blade.isLoading = false;
		    bladeNavigationService.setError('Error ' + error.status, $scope.blade);
		});
    }

    //Breadcrumps
    function setBreadcrumps() {
        //Clone array (angular.copy leaves the same reference)
        $scope.blade.breadcrumbs = $scope.blade.breadcrumbs.slice(0);

        //catalog breadcrump by default
        var breadCrumb = {
            id: $scope.blade.currentEntity.id,
            name: $scope.blade.currentEntity.displayName,
            blade: $scope.blade
        };

        //prevent dublicate items
        if (!_.some($scope.blade.breadcrumbs, function (x) { return x.id == breadCrumb.id; })) {
            $scope.blade.breadcrumbs.push(breadCrumb);
        }

        breadCrumb.navigate = function (breadcrumb) {
            bladeNavigationService.closeBlade($scope.blade,
                function () {
                    bladeNavigationService.showBlade($scope.blade, $scope.blade.parentBlade);
                    $scope.blade.refresh();
                });
        };
    }

    $scope.$watch('pageSettings.currentPage', function (newPage) {
        $scope.blade.refresh();
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
        }
        edit(listItem);
    };

    function edit(listItem) {
        closeChildrenBlades();

        if (listItem.memberType === 'Organization') {
            $scope.blade.showDetailBlade(listItem, listItem.displayName);
        }
        // else do nothing as customer is opened on selecting it.
    };

    $scope.blade.showDetailBlade = function (listItem, title) {
        $scope.blade.setSelectedNode(listItem);

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

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.delete = function () {
        if (isItemsChecked()) {
            deleteChecked();
        } else {
            var dialog = {
                id: "notifyNothingSelected",
                title: "Message",
                message: "Nothing selected. Check some Organizations or Customers first."
            };
            dialogService.showNotificationDialog(dialog);
        }

        preventOrganizationListingOnce = true;
    };

    function isItemsChecked() {
        return $scope.listEntries && _.any($scope.listEntries, function (x) { return x.selected; });
    }

    function deleteChecked() {
        var dialog = {
            id: "confirmDeleteItem",
            title: "Delete confirmation",
            message: "Are you sure you want to delete selected Organizations or Customers?",
            callback: function (remove) {
                if (remove) {
                    closeChildrenBlades();

                    var selection = _.where($scope.listEntries, { selected: true, memberType: 'Organization' });
                    var organizationIds = _.pluck(selection, 'id');
                    selection = _.where($scope.listEntries, { selected: true, memberType: 'Contact' });
                    var customerIds = _.pluck(selection, 'id');

                    if (organizationIds.length > 0) {
                        organizations.remove({ ids: organizationIds }, function (data) {
                            $scope.blade.refresh();
                        });
                    }
                    if (customerIds.length > 0) {
                        contacts.remove({ ids: customerIds }, function (data) {
                            $scope.blade.refresh();
                        });
                    }
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.blade.setSelectedNode = function (listItem) {
        selectedNode = listItem;
        $scope.selectedNodeId = selectedNode.id;
    };

    $scope.selectNode = function (listItem) {
        $scope.blade.setSelectedNode(listItem);

        if (listItem.memberType === 'Organization') {
            if (preventOrganizationListingOnce) {
                preventOrganizationListingOnce = false;
            } else {
                var newBlade = {
                    id: 'memberList',
                    breadcrumbs: $scope.blade.breadcrumbs,
                    subtitle: 'Browsing "' + listItem.displayName + '"',
                    currentEntity: listItem,
                    controller: $scope.blade.controller,
                    template: $scope.blade.template,
                    isClosingDisabled: true
                };

                bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
            }
        } else {
            $scope.blade.showDetailBlade(listItem, listItem.displayName);
        }
    };

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.blade.headIcon = 'fa fa-user';

    $scope.blade.toolbarCommands = [
        {
            name: "Refresh", icon: 'fa fa-refresh',
            executeMethod: function () {
                $scope.blade.refresh();
            },
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "Add", icon: 'fa fa-plus',
            executeMethod: function () {
                closeChildrenBlades();

                var newBlade = {
                    id: 'listItemChild',
                    title: 'New member',
                    subtitle: 'Choose new member type',
                    controller: 'virtoCommerce.customerModule.memberAddController',
                    template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/member-add.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'customer:manage'
        },
        {
            name: "Manage", icon: 'fa fa-edit',
            executeMethod: function () {
                // selected OR the first checked listItem
                edit(selectedNode || _.find($scope.listEntries, function (x) { return x.selected; }));
            },
            canExecuteMethod: function () {
                return selectedNode || isItemsChecked();
            },
            permission: 'customer:manage'
        },
        {
            name: "Delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteChecked();
            },
            canExecuteMethod: function () {
                return isItemsChecked();
            },
            permission: 'customer:manage'
        }
    ];

    $scope.checkAll = function (selected) {
        angular.forEach($scope.listEntries, function (item) {
            item.selected = selected;
        });
    };

    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //$scope.blade.refresh();
}]);
