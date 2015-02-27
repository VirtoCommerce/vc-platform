angular.module('virtoCommerce.orderModule.blades')
.controller('customerOrderItemsController', ['$scope', 'bladeNavigationService', 'dialogService', 'calculateTotalsService', function ($scope, bladeNavigationService, dialogService, calculateTotalsService) {
    //pagination settigs
	$scope.pageSettings = {};
	$scope.totals = {};
    $scope.pageSettings.totalItems = $scope.blade.currentEntity.items.length;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 10;

    var selectedNode = null;

    $scope.$watch("blade.currentEntity", function (operation) {
    	$scope.totals = calculateTotalsService.recalculateTotals(operation);
    }, true);

    $scope.blade.refresh = function () {
    	$scope.blade.isLoading = false;
    	$scope.blade.selectedAll = false;
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
        //    currentEntities: $scope.blade.currentEntity,
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
                            //var undeletedEntries = _.reject($scope.blade.currentEntity, function (x) { return x.selected; });
                            //items.updateitem({ id: $scope.blade.currentEntityId, associations: undeletedEntries }, function () {
                            //    $scope.blade.refresh();
                            //});
                        }
                    }
                }

                dialogService.showConfirmationDialog(dialog);
            },
            canExecuteMethod: function () {
                return _.any($scope.blade.currentEntity.items, function (x) { return x.selected; });;
            }
        }
    ];

    //$scope.$watch('pageSettings.currentPage', function (newPage) {
    //    $scope.blade.refresh();
    //});

    $scope.selectItem = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.id;
    };

    $scope.checkAll = function (selected) {
        angular.forEach($scope.blade.currentEntity.items, function (item) {
            item.selected = selected;
        });
    };

    $scope.blade.refresh();
}]);