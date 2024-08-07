angular.module('platformWebApp')
    .controller('platformWebApp.dynamicPropertyDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.settings', 'platformWebApp.dynamicProperties.api', 'platformWebApp.dynamicProperties.dictionaryItemsApi', 'platformWebApp.dynamicProperties.valueTypesService', function ($scope, bladeNavigationService, dialogService, settings, dynamicPropertiesApi, dictionaryItemsApi, valueTypesService) {
        var blade = $scope.blade;
        blade.updatePermission = 'platform:dynamic_properties:update';
        blade.headIcon = 'far fa-plus-square';
        blade.title = 'platform.blades.dynamicProperty-detail.title';
        var localDictionaryValues = [];

        blade.refresh = function () {
            //Actualize displayed names to correspond to system languages
            settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' }, function (languages) {
                blade.currentEntity = blade.isNew ? { valueType: 'ShortText', displayNames: [] } : blade.currentEntity;
                blade.currentEntity.displayNames = _.map(languages, function (x) {
                    var retVal = { locale: x };
                    var existName = _.find(blade.currentEntity.displayNames, function (y) { return y.locale.toLowerCase() == x.toLowerCase(); });
                    if (angular.isDefined(existName)) {
                        retVal = existName;
                    }
                    return retVal;
                });
                blade.origEntity = blade.currentEntity;
                blade.currentEntity = angular.copy(blade.origEntity);
                blade.isLoading = false;
            });
        };

        $scope.$watch('blade.currentEntity.valueType', function (newValue) {
            blade.hasMultivalue = blade.hasDictionary = blade.hasMultilanguage = true;

            switch (newValue) {
                case 'DateTime':
                case 'Boolean':
                case 'Image':
                    blade.hasMultivalue = blade.currentEntity.isArray = false;
                    blade.hasDictionary = blade.currentEntity.isDictionary = false;
                    blade.hasMultilanguage = blade.currentEntity.isMultilingual = false;
                    break;
                case 'Integer':
                case 'Decimal':
                    blade.hasDictionary = blade.currentEntity.isDictionary = false;
                    blade.hasMultilanguage = blade.currentEntity.isMultilingual = false;
                    break;
                case 'LongText':
                case 'Html':
                    blade.hasDictionary = blade.currentEntity.isDictionary = false;
                    blade.hasMultivalue = blade.currentEntity.isArray = false;
                    break;
            }
        });

        $scope.arrayFlagValidator = function (value) {
            return !value || blade.currentEntity.valueType === 'ShortText' || blade.currentEntity.valueType === 'Integer' || blade.currentEntity.valueType === 'Decimal';
        };

        $scope.multilingualFlagValidator = function (value) {
            return !value || blade.currentEntity.valueType === 'ShortText' || blade.currentEntity.valueType === 'LongText' || blade.currentEntity.valueType === 'Html';
        };

        $scope.dictionaryFlagValidator = function (value) {
            return !value || blade.currentEntity.valueType === 'ShortText';
        };

        $scope.openChild = function (childType) {
            var newBlade = {
                id: "propertyChild",
                currentEntity: blade.currentEntity
            };

            if (childType == 'dict') {
                newBlade.isApiSave = !blade.isNew;
                newBlade.controller = 'platformWebApp.propertyDictionaryController';
                newBlade.template = '$(Platform)/Scripts/app/dynamicProperties/blades/property-dictionary.tpl.html';
                if (blade.isNew) {
                    newBlade.data = localDictionaryValues;
                    newBlade.onChangesConfirmedFn = function (data) {
                        localDictionaryValues = data;
                    }
                }
            }

            bladeNavigationService.showBlade(newBlade, blade);
            $scope.currentChild = childType;
        }

        $scope.setForm = function (form) { $scope.formScope = form; }

        function isDirty() {
            return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
        }

        $scope.saveChanges = function () {
            if (blade.isNew) {
                blade.currentEntity.objectType = blade.objectType;
                dynamicPropertiesApi.save({}, blade.currentEntity,
                    function (data) {
                        blade.onChangesConfirmedFn(data);
                        // save dictionary items for new entity
                        if (data.isDictionary) {
                            localDictionaryValues.forEach(function (x) {
                                x.propertyId = data.id;
                            });

                            dictionaryItemsApi.save({}, localDictionaryValues,
                                function () {
                                    $scope.bladeClose();
                                    blade.parentBlade.refresh(true);
                                },
                                function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                        } else {
                            $scope.bladeClose();
                            blade.parentBlade.refresh(true);
                        }
                    },
                    function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            } else {
                dynamicPropertiesApi.update({}, blade.currentEntity,
                    function () {
                        angular.copy(blade.currentEntity, blade.origEntity);
                        blade.currentEntity = blade.origEntity;
                        if (blade.parentBlade.parentRefresh) {
                            blade.parentBlade.parentRefresh();
                        }
                        blade.refresh();
                    },
                    function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            }
        };

        function deleteEntry() {
            var dialog = {
                id: "confirmDelete",
                title: "platform.dialogs.dynamic-property-delete.title",
                message: "platform.dialogs.dynamic-property-delete.message",
                callback: function (remove) {
                    if (remove) {
                        dynamicPropertiesApi.delete({ propertyIds: [blade.currentEntity.id] },
                            function () {
                                $scope.bladeClose();
                                blade.parentBlade.refresh(true);
                            },
                            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                    }
                }
            }
            dialogService.showConfirmationDialog(dialog);
        }

        if (!blade.isNew) {
            blade.toolbarCommands = [
                {
                    name: "platform.commands.save", icon: 'fas fa-save',
                    executeMethod: function () {
                        $scope.saveChanges();
                    },
                    canExecuteMethod: function () {
                        return isDirty() && $scope.formScope && $scope.formScope.$valid;
                    },
                    permission: blade.updatePermission
                },
                {
                    name: "platform.commands.reset", icon: 'fa fa-undo',
                    executeMethod: function () {
                        angular.copy(blade.origEntity, blade.currentEntity);
                    },
                    canExecuteMethod: isDirty,
                    permission: blade.updatePermission
                },
                {
                    name: "platform.commands.delete", icon: 'fas fa-trash-alt',
                    executeMethod: deleteEntry,
                    canExecuteMethod: function () {
                        return !blade.isNew;
                    },
                    permission: 'platform:dynamic_properties:delete'
                }
            ];
        }

        blade.valueTypes = valueTypesService.query();

        // on load:
        blade.refresh();
    }]);
