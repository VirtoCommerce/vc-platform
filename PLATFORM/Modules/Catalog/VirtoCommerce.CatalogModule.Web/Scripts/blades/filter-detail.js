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
                $localStorage.catalogSearchFilters.push(blade.currentEntity);
                $localStorage.catalogSearchFilterId = blade.currentEntity.id;
                blade.parentBlade.filter.current = blade.currentEntity;
                blade.isNew = false;
            } else {
                angular.copy(blade.currentEntity, blade.origEntity);
            }
            blade.parentBlade.filter.criteriaChanged();
            // $scope.bladeClose();
        };

        function initializeBlade(data) {
            if (blade.isNew) data = { id: new Date().getTime(), name: 'Unnamed filter' };

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
                    name: "core.commands.apply-filter", icon: 'fa fa-filter',
                    executeMethod: function () {
                        $scope.saveChanges();
                    },
                    canExecuteMethod: function () {
                        return formScope && formScope.$valid;
                    }
                },
                {
                    name: "platform.commands.reset", icon: 'fa fa-undo',
                    executeMethod: function () {
                        angular.copy(blade.origEntity, blade.currentEntity);
                    },
                    canExecuteMethod: isDirty
                },
                {
                    name: "platform.commands.delete", icon: 'fa fa-trash-o',
                    executeMethod: deleteEntry,
                    canExecuteMethod: function () {
                        return true;
                    }
                }];


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

        // actions on load        
        initializeBlade(blade.data);
    }]);
