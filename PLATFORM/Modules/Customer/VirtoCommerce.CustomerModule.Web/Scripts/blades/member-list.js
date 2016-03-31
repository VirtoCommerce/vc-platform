angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberListController', ['$scope', 'virtoCommerce.customerModule.members', 'platformWebApp.dialogService', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper', 'virtoCommerce.customerModule.memberTypesResolverService',
function ($scope, members, dialogService, bladeUtils, uiGridHelper, memberTypesResolverService) {
    $scope.uiGridConstants = uiGridHelper.uiGridConstants;

    var blade = $scope.blade;
    blade.title = 'customer.blades.member-list.title';
    var bladeNavigationService = bladeUtils.bladeNavigationService;

    blade.refresh = function () {
        blade.isLoading = true;
        members.search(
        {
            memberId: blade.currentEntity.id,
            keyword: filter.keyword ? filter.keyword : undefined,
            sort: uiGridHelper.getSortExpression($scope),
            skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
            take: $scope.pageSettings.itemsPerPageCount
        },
            function (data) {
                blade.isLoading = false;
                $scope.pageSettings.totalItems = data.totalCount;
                // precalculate icon
                var memberTypeDefinition;
                _.each(data.members, function (x) {
                    if (memberTypeDefinition = _.findWhere(memberTypesResolverService.objects, { memberType: x.memberType })) {
                        x._memberTypeIcon = memberTypeDefinition.icon;
                    }
                });
                $scope.listEntries = data.members;

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
                var breadCrumb = generateBreadcrumb(blade.currentEntity.id, blade.currentEntity.name);
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

    blade.showDetailBlade = function (listItem, title) {
        blade.setSelectedNode(listItem);

        var memberTypeDefinition = _.findWhere(memberTypesResolverService.objects, { memberType: listItem.memberType });
        if (memberTypeDefinition) {
            var newBlade = {
                id: "listMemberDetail",
                currentEntity: listItem,
                title: title,
                memberTypeDefinition: memberTypeDefinition,
                controller: memberTypeDefinition.controller,
                template: memberTypeDefinition.template
            };
            bladeNavigationService.showBlade(newBlade, blade);
        } else {
            dialogService.showNotificationDialog({
                id: "error",
                title: "customer.dialogs.unknown-member-type.title",
                message: "customer.dialogs.unknown-member-type.message",
                messageValues: { memberType: blade.currentEntity.memberType },
            });
        }
    };

    $scope.delete = function (data) {
        deleteList([data]);
    };

    function deleteList(selection) {
        var dialog = {
            id: "confirmDeleteItem",
            title: "customer.dialogs.members-delete.title",
            message: "customer.dialogs.members-delete.message",
            callback: function (remove) {
                if (remove) {
                    bladeNavigationService.closeChildrenBlades(blade, function () {
                        var memberIds = _.pluck(selection, 'id');

                        if (_.any(memberIds)) {
                            members.remove({ ids: memberIds },
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
        $scope.selectedNodeId = listItem.id;
    };

    $scope.selectNode = function (listItem) {
        blade.setSelectedNode(listItem);

        if (listItem.memberType === 'Organization') {
            var newBlade = {
                id: blade.id,
                breadcrumbs: blade.breadcrumbs,
                subtitle: 'customer.blades.member-list.subtitle',
                subtitleValues: { name: listItem.name },
                currentEntity: listItem,
                disableOpenAnimation: true,
                controller: blade.controller,
                template: blade.template,
                isClosingDisabled: true
            };
            bladeNavigationService.showBlade(newBlade, blade.parentBlade);
        } else {
            blade.showDetailBlade(listItem, listItem.name);
        }
    };

    blade.headIcon = 'fa-user';

    blade.toolbarCommands = [
        {
            name: "platform.commands.refresh", icon: 'fa fa-refresh',
            executeMethod: blade.refresh,
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "platform.commands.add", icon: 'fa fa-plus',
            executeMethod: function () {
                var newBlade = {
                    id: 'listItemChild',
                    title: 'customer.blades.member-add.title',
                    subtitle: 'customer.blades.member-add.subtitle',
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
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: function () { deleteList($scope.gridApi.selection.getSelectedRows()); },
            canExecuteMethod: function () {
                return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
            },
            permission: 'customer:delete'
        }
    ];


    // filtering
    var filter = $scope.filter = {};

    filter.criteriaChanged = function () {
        if ($scope.pageSettings.currentPage > 1) {
            $scope.pageSettings.currentPage = 1;
        } else {
            blade.refresh();
        }
    };

    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
            uiGridHelper.bindRefreshOnSortChanged($scope);
        });

        bladeUtils.initializePagination($scope);
    };


    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //blade.refresh();
}]);
