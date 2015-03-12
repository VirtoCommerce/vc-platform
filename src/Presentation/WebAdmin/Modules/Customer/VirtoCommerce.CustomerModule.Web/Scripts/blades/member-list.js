angular.module('virtoCommerce.customerModule.blades')
.controller('memberListController', ['$scope', 'members', 'bladeNavigationService', 'dialogService', function ($scope, members, bladeNavigationService, dialogService) {
    //pagination settigs
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.listEntriesPerPageCount = 20;

    $scope.filter = { searchKeyword: undefined };

    $scope.pageSettings.selectedAll = false;
    var selectedNode = null;
    var preventOrganizationListingOnce; // prevent from unwanted additional actions after command was activated from context menu
    $scope.blade.title = 'Organizations & Customers';

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        members.search(
            {
                //catalog: $scope.blade.currentEntityId,
                //category: $scope.blade.categoryId,
                keyword: $scope.filter.searchKeyword,
                // respGroup: 'withOrganizations, withContacts',
                start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.listEntriesPerPageCount,
                count: $scope.pageSettings.listEntriesPerPageCount
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
		    // setBreadcrumps();
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
            id: $scope.blade.currentEntityId,
            // name: $scope.blade.catalog.displayName,
            name: '???.displayName',
            blade: $scope.blade
        };

        //if organization need change to organization breadcrumb
        if (angular.isDefined($scope.blade.organization)) {

            breadCrumb.id = $scope.blade.organizationId;
            breadCrumb.name = $scope.blade.organization.displayName;
        }

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
        if (listItem.memberType === 'organization') {
            preventOrganizationListingOnce = true;
        }
        edit(listItem);
    };

    function edit(listItem) {
        closeChildrenBlades();

        $scope.selectedItem = listItem;
        if (listItem.memberType === 'Organization') {
            $scope.blade.showOrganizationBlade(listItem.id, listItem.displayName);
        }
        // else do nothing as item is opened on selecting it.
    };

    $scope.blade.showOrganizationBlade = function (id, title) {
        var newBlade = {
            id: "listMemberDetail",
            currentEntityId: id,
            currentEntity: $scope.selectedItem,
            isOrganization: true,
            title: title,
            subtitle: 'Organization details',
            controller: 'memberDetailController',
            template: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/blades/organization-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.blade.showContactBlade = function (id, title) {
        var newBlade = {
            id: "listMemberDetail",
            currentEntityId: id,
            currentEntity: $scope.selectedItem,
            isOrganization: false,
            title: title,
            subtitle: 'Customer details',
            controller: 'memberDetailController',
            template: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/blades/member-contact-detail.tpl.html'
        };
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

                    //var selection = $filter('filter')($scope.listEntries, { selected: true }, true);

                    //var listEntryLinks = [];
                    //var categoryIds = [];
                    //var itemIds = [];
                    angular.forEach(selection, function (listItem) {
                        var deletingLink = false;

                        //if (listItem.memberType === 'category') {
                        //    if ($scope.blade.catalog.virtual && _.some(listItem.links, function (x) { return x.categoryId === $scope.blade.categoryId; })) {
                        //        deletingLink = true;
                        //    } else {
                        //        categoryIds.push(listItem.id);
                        //    }
                        //} else {
                        //    if ($scope.blade.catalog.virtual) {
                        //        deletingLink = true;
                        //    } else {
                        //        itemIds.push(listItem.id);
                        //    }
                        //}

                        if (deletingLink)
                            listEntryLinks.push({
                                listEntryId: listItem.id,
                                listEntryType: listItem.memberType,
                                catalogId: $scope.blade.currentEntityId,
                                categoryId: $scope.blade.categoryId,
                            });
                    });

                    //if (listEntryLinks.length > 0) {
                    //    customers.deletelinks(listEntryLinks, function (data, headers) {
                    //        $scope.blade.refresh();
                    //    });
                    //}
                    //if (categoryIds.length > 0) {
                    //    categories.remove({ ids: categoryIds }, function (data, headers) {
                    //        $scope.blade.refresh();
                    //    });
                    //}
                    //if (itemIds.length > 0) {
                    //    items.remove({ ids: itemIds }, function (data, headers) {
                    //        $scope.blade.refresh();
                    //    });
                    //}
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
                    currentEntityId: $scope.blade.currentEntityId,
                    catalog: $scope.blade.catalog,
                    //categoryId: listItem.id,
                    //category: listItem,
                    controller: 'memberListController',
                    template: $scope.blade.template
                };

                if (e.ctrlKey) {
                    bladeNavigationService.showBlade(newBlade, $scope.blade);
                }
                else {
                    bladeNavigationService.closeBlade($scope.blade, function () {
                        bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
                    });
                }
            }
        } else {
            $scope.blade.showContactBlade(listItem.id, listItem.displayName);
        }

        //$scope.blade.currentItemId = selectedNode.memberType === 'Contact' ? selectedNode.id : undefined;
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

    $scope.bladeToolbarCommands = [
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
                      controller: 'memberAddController',
                      template: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/blades/member-add.tpl.html'
                  };
                  bladeNavigationService.showBlade(newBlade, $scope.blade);
              },
              canExecuteMethod: function () {
                  return true;
              }
          },
            {
                name: "Manage", icon: 'fa fa-edit',
                executeMethod: function () {
                    // selected OR the first checked listItem
                    edit(selectedNode || _.find($scope.listEntries, function (x) { return x.selected; }));
                },
                canExecuteMethod: function () {
                    return selectedNode || isItemsChecked();
                }
            },
            {
                name: "Delete", icon: 'fa fa-trash-o',
                executeMethod: function () {
                    deleteChecked();
                },
                canExecuteMethod: function () {
                    return isItemsChecked();
                }
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
