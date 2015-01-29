angular.module('virtoCommerce.orderModule.blades')
.controller('customerOrderItemsController', ['$scope', 'bladeNavigationService', 'dialogService', 'items', function ($scope, bladeNavigationService, dialogService, items) {
    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        $scope.blade.parentBlade.refresh().$promise.then(function (data) {
            initializeBlade(data.associations);
        });
    };

    function initializeBlade(data) {
        $scope.selectedAll = false;
        $scope.blade.currentEntities = angular.copy(data);
        $scope.blade.origItem = data;
        $scope.blade.isLoading = false;
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

    function openAddEntityWizard() {
        //var newBlade = {
        //    id: "customerOrderItemWizard",
        //    currentEntities: $scope.blade.currentEntities,
        //    title: "New Associations",
        //    //subtitle: '',
        //    controller: 'customerOrderItemWizardController',
        //    template: 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/blades/customerOrder-items.tpl.html'
        //};
        //bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.bladeToolbarCommands = [
        {
            name: "Add item", icon: 'fa fa-plus',
            executeMethod: function () {
                openAddEntityWizard();
            },
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "Remove", icon: 'fa fa-trash-o',
            executeMethod: function () {
                var dialog = {
                    id: "confirmDeleteItem",
                    title: "Remove confirmation",
                    message: "Are you sure you want to remove selected items?",
                    callback: function (remove) {
                        if (remove) {
                            //var undeletedEntries = _.reject($scope.blade.currentEntities, function (x) { return x.selected; });
                            //items.updateitem({ id: $scope.blade.currentEntityId, associations: undeletedEntries }, function () {
                            //    $scope.blade.refresh();
                            //});
                        }
                    }
                }

                dialogService.showConfirmationDialog(dialog);
            },
            canExecuteMethod: function () {
                return _.any($scope.blade.currentEntities, function (x) { return x.selected; });;
            }
        }
    ];

    $scope.checkAll = function (selected) {
        angular.forEach($scope.blade.currentEntities, function (item) {
            item.selected = selected;
        });
    };

    initializeBlade($scope.blade.currentEntities);
}]);
