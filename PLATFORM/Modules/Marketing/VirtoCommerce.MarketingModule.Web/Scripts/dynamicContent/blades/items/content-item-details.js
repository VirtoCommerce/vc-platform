angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addContentItemsController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.contentItems', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.dynamicProperties.dictionaryItemsApi', 'platformWebApp.settings',
    function ($scope, marketing_dynamicContents_res_contentItems, bladeNavigationService, dialogService, dictionaryItemsApi, settings) {
        $scope.setForm = function (form) {
            $scope.formScope = form;
        }

        var blade = $scope.blade;
        blade.updatePermission = 'marketing:update';

        blade.initialize = function () {
            blade.toolbarCommands = [];

            if (!blade.isNew) {
                blade.toolbarCommands = [
                    {
                        name: "platform.commands.save", icon: 'fa fa-save',
                        executeMethod: function () {
                            blade.saveChanges();
                        },
                        canExecuteMethod: function () {
                            return !angular.equals(blade.originalEntity, blade.entity) && !$scope.formScope.$invalid;
                        },
                        permission: blade.updatePermission
                    },
                    {
                        name: "platform.commands.reset", icon: 'fa fa-undo',
                        executeMethod: function () {
                            blade.entity = angular.copy(blade.originalEntity);
                        },
                        canExecuteMethod: function () {
                            return !angular.equals(blade.originalEntity, blade.entity);
                        },
                        permission: blade.updatePermission
                    },
                    {
                        name: "platform.commands.delete", icon: 'fa fa-trash',
                        executeMethod: function () {
                            var dialog = {
                                id: "confirmDeleteContentItem",
                                title: "marketing.dialogs.content-item-delete.title",
                                message: "marketing.dialogs.content-item-delete.message",
                                callback: function (remove) {
                                    if (remove) {
                                        blade.delete();
                                    }
                                }
                            };

                            dialogService.showConfirmationDialog(dialog);
                        },
                        canExecuteMethod: function () {
                            return true;
                        },
                        permission: blade.updatePermission
                    }
                ];
            }

            blade.toolbarCommands.push(
                {
                    name: "marketing.commands.manage-type-properties", icon: 'fa fa-edit',
                    executeMethod: function () {
                        var newBlade = {
                            id: 'dynamicPropertyList',
                            objectType: blade.entity.objectType,
                            controller: 'platformWebApp.dynamicPropertyListController',
                            template: '$(Platform)/Scripts/app/dynamicProperties/blades/dynamicProperty-list.tpl.html'
                        };
                        bladeNavigationService.showBlade(newBlade, blade);
                    },
                    canExecuteMethod: function () {
                        return angular.isDefined(blade.entity.objectType);
                    }
                });

            blade.originalEntity = angular.copy(blade.entity);

            blade.isLoading = false;
        }

        blade.delete = function () {
            blade.isLoading = true;
            marketing_dynamicContents_res_contentItems.delete({ ids: [blade.entity.id] }, function () {
                blade.parentBlade.initializeBlade();
                bladeNavigationService.closeBlade(blade);
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
        }

        blade.saveChanges = function () {
            blade.isLoading = true;

            if (blade.isNew) {
                marketing_dynamicContents_res_contentItems.save({}, blade.entity, function (data) {
                    blade.parentBlade.initializeBlade();
                    bladeNavigationService.closeBlade(blade);
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
            }
            else {
                marketing_dynamicContents_res_contentItems.update({}, blade.entity, function (data) {
                    blade.parentBlade.initializeBlade();
                    blade.originalEntity = angular.copy(blade.entity);
                    blade.isLoading = false;
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
            }
        }

        $scope.editDictionary = function (property) {
            var newBlade = {
                id: "propertyDictionary",
                isApiSave: true,
                currentEntity: property,
                controller: 'platformWebApp.propertyDictionaryController',
                template: '$(Platform)/Scripts/app/dynamicProperties/blades/property-dictionary.tpl.html',
                onChangesConfirmedFn: function () {
                    blade.entity.dynamicProperties = angular.copy(blade.entity.dynamicProperties);
                }
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };

        $scope.getDictionaryValues = function (property, callback) {
            dictionaryItemsApi.query({ id: property.objectType, propertyId: property.id }, callback);
        }

        blade.headIcon = 'fa-inbox';

        settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' }, function (data) {
            $scope.languages = data;
        });

        blade.initialize();
    }]);