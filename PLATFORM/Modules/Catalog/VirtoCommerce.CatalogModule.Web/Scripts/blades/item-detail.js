angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', 'virtoCommerce.catalogModule.items', function ($scope, bladeNavigationService, settings, items) {
    var blade = $scope.blade;
    blade.updatePermission = 'catalog:update';
    blade.currentEntityId = blade.itemId;

    blade.refresh = function (parentRefresh) {
        blade.isLoading = true;

        return items.get({ id: blade.itemId }, function (data) {
            blade.itemId = data.id;
            blade.title = data.code;
            blade.securityScopes = data.securityScopes;
            if (!data.productType) {
                data.productType = 'Physical';
            }
            blade.subtitle = data.productType + ' item details';
            $scope.isTitular = data.titularItemId == null;
            $scope.isTitularConfirmed = $scope.isTitular;

            blade.item = angular.copy(data);
            blade.origItem = data;
            blade.isLoading = false;
            if (parentRefresh && blade.parentBlade.refresh) {
                blade.parentBlade.refresh();
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }

    //$scope.onTitularChange = function () {
    //    $scope.isTitular = !$scope.isTitular;
    //    if ($scope.isTitular) {
    //        blade.item.titularItemId = null;
    //    } else {
    //        blade.item.titularItemId = blade.origItem.titularItemId;
    //    }
    //};

    $scope.codeValidator = function (value) {
        var pattern = /[$+;=%{}[\]|\\\/@ ~!^*&()?:'<>,]/;
        return !pattern.test(value);
    };

    function isDirty() {
        return !angular.equals(blade.item, blade.origItem) && blade.hasUpdatePermission();
    };

    function canSave() {
        return isDirty() && formScope && formScope.$valid;
    }

    function saveChanges() {
        blade.isLoading = true;
        items.update({}, blade.item, function () {
            blade.refresh(true);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, saveChanges, closeCallback, "catalog.dialogs.item-save.title", "catalog.dialogs.item-save.message");
    };

    var formScope;
    $scope.setForm = function (form) { formScope = form; }

    blade.headIcon = blade.productType === 'Digital' ? 'fa-file-zip-o' : 'fa-dropbox';

    blade.toolbarCommands = [
	    {
	        name: "platform.commands.save", icon: 'fa fa-save',
	        executeMethod: saveChanges,
	        canExecuteMethod: canSave,
	        permission: blade.updatePermission
	    },
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origItem, blade.item);
                $scope.isTitular = blade.item.titularItemId == null;
            },
            canExecuteMethod: isDirty,
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.clone", icon: 'fa fa-files-o',
            executeMethod: function () {
                blade.isLoading = true;
                items.cloneItem({ itemId: blade.itemId }, function (data) {
                    var newBlade = {
                        id: blade.id,
                        item: data,
                        title: "catalog.wizards.new-product.title",
                        subtitle: 'catalog.wizards.new-product.subtitle',
                        controller: 'virtoCommerce.catalogModule.newProductWizardController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/newProduct/new-product-wizard.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, blade.parentBlade);
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });

            },
            canExecuteMethod: function () { return !isDirty(); },
            permission: 'catalog:create'
        }
    ];

    // datepicker
    $scope.datepickers = {}
    $scope.open = function ($event, which) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.datepickers[which] = true;
    };
    // $scope.dateOptions = { 'year-format': "'yyyy'" };

    $scope.openDictionarySettingManagement = function () {
        var newBlade = {
            id: 'settingDetailChild',
            isApiSave: true,
            currentEntityId: 'VirtoCommerce.Core.General.TaxTypes',
            parentRefresh: function (data) { $scope.taxTypes = data; },
            controller: 'platformWebApp.settingDictionaryController',
            template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
    
    $scope.$on("refresh-entity-by-id", function (event, id) {
        if (blade.currentEntityId === id) {
            blade.refresh();
        }
    });

    $scope.taxTypes = settings.getValues({ id: 'VirtoCommerce.Core.General.TaxTypes' });
    blade.refresh(false);
}]);
