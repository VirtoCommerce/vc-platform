angular.module('catalogModule.blades.itemDetail', [])
.controller('itemDetailController', ['$rootScope', '$scope', 'bladeNavigationService', '$injector', 'items', 'dialogService', function ($rootScope, $scope, bladeNavigationService, $injector, items, dialogService) {
    $scope.currentBlade = $scope.blade;
    $scope.currentBlade.origItem = {};
    $scope.currentBlade.item = {};

    $scope.currentBlade.refresh = function (parentRefresh) {
        items.get({ id: $scope.currentBlade.itemId }, function (data) {
            $scope.currentBlade.itemId = data.id;
            $scope.currentBlade.title = data.code;
            $scope.isTitular = data.titularItemId == null;
            $scope.isTitularConfirmed = $scope.isTitular;

            $scope.currentBlade.item = angular.copy(data);
            $scope.currentBlade.origItem = data;
            $scope.currentBlade.isLoading = false;
            if (parentRefresh) {
                $scope.currentBlade.parentBlade.refresh();
            }

        });
    }


    $scope.setTitular = function () {
        $scope.currentBlade.item.titularItemId = null;
        $scope.isTitular = true;
    };

    function isDirty() {
        return !angular.equals($scope.currentBlade.item, $scope.currentBlade.origItem);
    };

    function saveChanges() {
        $scope.currentBlade.isLoading = true;
        var changes = { id: $scope.currentBlade.item.id, name: $scope.currentBlade.item.name, titularItemId: $scope.currentBlade.item.titularItemId, code: $scope.currentBlade.item.code };
        items.updateitem({}, $scope.currentBlade.item, function (data, headers) {
            $scope.currentBlade.refresh(true);
        });
    };

    function closeThisBlade(closeCallback) {
        if ($scope.currentBlade.childrenBlades.length > 0) {
            var callback = function () {
                if ($scope.currentBlade.childrenBlades.length == 0) {
                    closeCallback();
                };
            };
            angular.forEach($scope.currentBlade.childrenBlades, function (child) {
                bladeNavigationService.closeBlade(child, callback);
            });
        }
        else {
            closeCallback();
        }
    };

    $scope.currentBlade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The item has been modified. Do you want to save changes?"
            };
            dialog.callback = function (needSave) {
                if (needSave) {
                    saveChanges();
                }
                closeThisBlade(closeCallback);
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeThisBlade(closeCallback);
        }
    };

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    $scope.bladeToolbarCommands = [
	 {
	     name: "Save", icon: 'icon-floppy',
	     executeMethod: function () {
	         saveChanges();
	     },
	     canExecuteMethod: function () {
	         return isDirty() && formScope && formScope.$valid;
	     }
	 },
        {
            name: "Reset", icon: 'icon-undo',
            executeMethod: function () {
                angular.copy($scope.currentBlade.origItem, $scope.currentBlade.item);
                $scope.isTitular = $scope.currentBlade.item.titularItemId == null;
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
	     {
	         name: "New variation", icon: 'icon-plus',
	         executeMethod: function () {
	             items.newVariation({ itemId: $scope.currentBlade.item.id }, function (data, headers) {
	                 $scope.currentBlade.refresh(true);

	                 var blade = {
	                     id: 'variationDetail',
	                     itemId: data.id,
	                     title: data.code,
	                     style: 'gray',
	                     subtitle: 'Variation details',
	                     controller: 'itemDetailController',
	                     template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/item-detail.tpl.html'
	                 };
	                 bladeNavigationService.showBlade(blade, $scope.currentBlade);
	             });
	         },
	         canExecuteMethod: function () {
	             return $scope.isTitularConfirmed;
	         }
	     }
    ];

    $scope.toolbarTemplate = "Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/item-detail-toolbar.tpl.html";


    $scope.currentBlade.refresh(false);
}]);
