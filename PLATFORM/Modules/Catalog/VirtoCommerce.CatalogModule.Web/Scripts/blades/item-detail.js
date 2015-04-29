angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemDetailController', ['$rootScope', '$scope', 'bladeNavigationService', '$injector', 'virtoCommerce.catalogModule.items', 'dialogService', function ($rootScope, $scope, bladeNavigationService, $injector, items, dialogService) {
    $scope.currentBlade = $scope.blade;
    $scope.currentBlade.origItem = {};
    $scope.currentBlade.item = {};
    $scope.blade.currentEntityId = $scope.currentBlade.itemId;

    $scope.currentBlade.refresh = function (parentRefresh) {
        $scope.currentBlade.isLoading = true;

        return items.get({ id: $scope.currentBlade.itemId }, function (data) {
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

    $scope.onTitularChange = function () {
        $scope.isTitular = !$scope.isTitular;
        if ($scope.isTitular) {
            $scope.currentBlade.item.titularItemId = null;
        } else {
            $scope.currentBlade.item.titularItemId = $scope.currentBlade.origItem.titularItemId;
        }
    };

    function isDirty() {
    	var retVal = !angular.equals($scope.currentBlade.item, $scope.currentBlade.origItem);
      	return retVal;
    };

    function saveChanges() {
        $scope.currentBlade.isLoading = true;
        var changes = { id: $scope.currentBlade.item.id, name: $scope.currentBlade.item.name, titularItemId: $scope.currentBlade.item.titularItemId, code: $scope.currentBlade.item.code };
        items.updateitem({}, $scope.currentBlade.item, function (data, headers) {
            $scope.currentBlade.refresh(true);
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
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
	     name: "Save", icon: 'fa fa-save',
	     executeMethod: function () {
	         saveChanges();
	     },
	     canExecuteMethod: function () {
	         return isDirty() && formScope && formScope.$valid;
	     },
	     permission: 'catalog:items:manage'
	 },
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.currentBlade.origItem, $scope.currentBlade.item);
                $scope.isTitular = $scope.currentBlade.item.titularItemId == null;
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'catalog:items:manage'
        },
	     {
	         name: "New variation", icon: 'fa fa-plus',
	         executeMethod: function () {

	             items.newVariation({ itemId: $scope.currentBlade.item.id }, function (data, headers) {
	                 var blade = {
	                     id: "newVariationWizard",
	                     item: data,
	                     title: "New variation",
	                     subtitle: 'Fill all variation information',
	                     controller: 'virtoCommerce.catalogModule.newProductWizardController',
	                     template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/newProduct/new-variation-wizard.tpl.html'
	                 };
	                 bladeNavigationService.showBlade(blade, $scope.currentBlade);
	             });
	         },
	         canExecuteMethod: function () {
	             return $scope.isTitularConfirmed;
	         },
	         permission: 'catalog:items:manage'
	     }
    ];


    $scope.currentBlade.refresh(false);
    $scope.toolbarTemplate = "Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail-toolbar.tpl.html";
}]);
