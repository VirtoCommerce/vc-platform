angular.module('virtoCommerce.customerModule.blades', [
   'virtoCommerce.customerModule.resources'
])
.controller('customerListController', ['$scope', 'members', 'bladeNavigationService', 'dialogService',
function ($scope, members, bladeNavigationService, dialogService) {
    //pagination settigs
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 20;

    $scope.filter = { searchKeyword: undefined };

    $scope.selectedAll = false;
    var selectedNode = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        members.search({
            keyword: $scope.filter.searchKeyword,
            start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
            count: $scope.pageSettings.itemsPerPageCount
        }, function (data) {
            $scope.blade.isLoading = false;
            $scope.selectedAll = false;

            $scope.pageSettings.totalItems = angular.isDefined(data.totalCount) ? data.totalCount : 0;
            $scope.objects = data.contacts;

            if (selectedNode != null) {
                //select the node in the new list
                angular.forEach(data.contacts, function (node) {
                    if (selectedNode.id === node.id) {
                        selectedNode = node;
                    }
                });
            }
        });
    };

    $scope.$watch('pageSettings.currentPage', function (newPage) {
        $scope.blade.refresh();
    });

    $scope.selectNode = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.id;

        var newBlade = {
            id: 'customerDetail',
            title: selectedNode.fullName,
            subtitle: 'Customer details',
            currentEntityId: selectedNode.id,
            controller: 'customerDetailController',
            template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/customer-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.checkAll = function (selected) {
        angular.forEach($scope.objects, function (item) {
            item.selected = selected;
        });
    };

    //function isItemsChecked() {
    //    return $scope.objects && _.any($scope.objects, function (x) { return x.selected; });
    //}

    //function deleteChecked() {
    //    var dialog = {
    //        id: "confirmDeleteItem",
    //        title: "Delete confirmation",
    //        message: "Are you sure you want to delete selected members?",
    //        callback: function (remove) {
    //            if (remove) {
    //                closeChildrenBlades();

    //                var selection = _.where($scope.objects, { selected: true });
    //                var itemIds = _.pluck(selection, 'id');
    //                members.remove({ ids: itemIds }, function (data, headers) {
    //                    $scope.blade.refresh();
    //                });
    //            }
    //        }
    //    }
    //    dialogService.showConfirmationDialog(dialog);
    //}

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
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

    $scope.bladeHeadIco = 'fa fa-user';

    $scope.bladeToolbarCommands = [
          {
              name: "Refresh", icon: 'fa fa-refresh',
              executeMethod: function () {
                  $scope.blade.refresh();
              },
              canExecuteMethod: function () {
                  return true;
              }
          }
          //{
          //    name: "Delete", icon: 'fa fa-trash-o',
          //    executeMethod: function () {
          //        deleteChecked();
          //    },
          //    canExecuteMethod: function () {
          //        return isItemsChecked();
          //    }
          //}
    ];


    // actions on load
    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //$scope.blade.refresh();
    var blade = {
        id: 'memberList',
        title: 'Organizations & Customers',
        controller: 'memberListController',
        template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/member-list.tpl.html',
        isClosingDisabled: true
    };
    bladeNavigationService.showBlade(blade);
}]);