angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.filterDetailController', ['$scope', '$localStorage', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService',
    function ($scope, $localStorage, dialogService, bladeNavigationService) {
        var blade = $scope.blade;

        $scope.trueFalse = [
            { name: 'True', value: true },
            { name: 'False', value: false }
        ];

        $scope.itemTypes = [
            { name: 'Physical', value: 'Physical' },
            { name: 'Digital', value: 'Digital' }
        ];

        $scope.saveChanges = function () {
            if (blade.isNew) {
                // angular.copy(blade.currentEntity, blade.origEntity);
                $localStorage.catalogSearchFilters.push(blade.currentEntity);
                blade.parentBlade.filter.current = blade.currentEntity;
            } else {
                angular.copy(blade.currentEntity, blade.origEntity);
            }
            $scope.bladeClose();
            blade.parentBlade.refresh();
        };

        function initializeBlade(data) {
            if (blade.isNew) data = { id: new Date().getTime() };

            blade.currentEntity = angular.copy(data);
            blade.origEntity = data;
            blade.isLoading = false;

            blade.title = blade.isNew ? 'catalog.blades.filter-detail.new-title' : data.name;
            blade.subtitle = blade.isNew ? 'catalog.blades.filter-detail.new-subtitle' : 'catalog.blades.filter-detail.subtitle';
        };

        var formScope;
        $scope.setForm = function (form) {
            formScope = form;
        }

        function isDirty() {
            return !angular.equals(blade.currentEntity, blade.origEntity);
        };

        blade.headIcon = 'fa-filter';

        blade.toolbarCommands = [
                {
                    name: "catalog.commands.apply-filter", icon: 'fa fa-filter',
                    executeMethod: function () {
                        blade.parentBlade.filter.current = blade.currentEntity;
                        blade.parentBlade.refresh();
                    },
                    canExecuteMethod: function () { return true; }
                }];

        if (!blade.isNew)
            blade.toolbarCommands.splice(1, 0, {
                name: "platform.commands.save", icon: 'fa fa-save',
                executeMethod: function () {
                    $scope.saveChanges();
                },
                canExecuteMethod: function () {
                    return isDirty() && formScope && formScope.$valid;
                }
            },
            {
                name: "platform.commands.reset", icon: 'fa fa-undo',
                executeMethod: function () {
                    angular.copy(blade.origEntity, blade.currentEntity);
                },
                canExecuteMethod: function () {
                    return isDirty();
                }
            },
            {
                name: "platform.commands.delete", icon: 'fa fa-trash-o',
                executeMethod: deleteEntry,
                canExecuteMethod: function () {
                    return true;
                }
            });

        function deleteEntry() {
            //var dialog = {
            //    id: "confirmDelete",
            //    title: "-delete.title",
            //message: "-delete.message",
            //callback: function (remove) {
            //    if (remove) {
            //        blade.isLoading = true;
            blade.parentBlade.filter.current = null;
            $localStorage.catalogSearchFilters.splice($localStorage.catalogSearchFilters.indexOf(blade.origEntity), 1);
            delete $localStorage.catalogSearchFilterId;
            blade.parentBlade.refresh();
            $scope.bladeClose();
            //        }
            //    }
            //}
            //dialogService.showConfirmationDialog(dialog);
        }

        //blade.onClose = function (closeCallback) {
        //    if (isDirty()) {
        //        var dialog = {
        //            id: "confirmItemChange",
        //            title: "-save.title",
        //            message: "-save.message",
        //            callback: function (needSave) {
        //                if (needSave) {
        //                    $scope.saveChanges();
        //                }
        //                closeCallback();
        //            }
        //        };
        //        dialogService.showConfirmationDialog(dialog);
        //    }
        //    else {
        //        closeCallback();
        //    }
        //};

        // actions on load        
        initializeBlade(blade.data);
    }]);
