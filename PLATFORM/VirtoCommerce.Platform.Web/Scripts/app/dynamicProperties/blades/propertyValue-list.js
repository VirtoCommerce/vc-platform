angular.module('platformWebApp')
.controller('platformWebApp.propertyValueListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.settings', function ($scope, bladeNavigationService, dialogService, settings) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-plus-square-o';

    function initializeBlade(dynPropertyValues) {
        blade.data = dynPropertyValues;
        //var properties = _.pluck(dynPropertyValues, 'property');

        //var selectedProps = _.where(properties, { valueType: 'Decimal' });
        //_.forEach(selectedProps, function (prop) {
        //    prop.value = parseFloat(prop.value);
        //});

        var selectedProps = _.filter(dynPropertyValues, function (x) { return x.property.isMultilingual; });
        if (selectedProps.length > 0) {
            var groupedByProperty = _.groupBy(selectedProps, function (x) { return x.property.id; });
            //$scope.groupedValues = _.map(groupedByProperty, function (values, key) {
            //    return { alias: key, values: values };
            //});

            // load all languages and generate missing value wrappers
            settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' }, function (promiseData) {
                promiseData.sort();

                var initializationValues = _.filter(dynPropertyValues, function (x) { return !x.property.isMultilingual; });
                // generating multiple inputs inside single directive
                _.each(groupedByProperty, function (values) {
                    var localizedValues = [];

                    _.each(values, function (value) {
                        if (value.locale) {
                            localizedValues.push({ values: _.map(value.values, function (x) { return { value: x } }), locale: value.locale });
                        }
                    });

                    _.each(promiseData, function (x) {
                        if (_.all(localizedValues, function (val) { return val.locale !== x; })) {
                            localizedValues.push({ values: [], locale: x });
                        }
                    });

                    initializationValues.push({
                        property: values[0].property,
                        values: localizedValues
                    });
                });
                //// generating multiple inputs and directives
                //_.each(groupedByProperty, function (values) {
                //    _.each(values, function (value) {
                //        if (value.locale) {
                //            initializationValues.push(value);
                //        } else {
                //            _.each(promiseData, function (x) {
                //                initializationValues.push({
                //                    property: value.property,
                //                    locale: x,
                //                    values: []
                //                });
                //            });
                //        }
                //    });
                //});

                initializeBlade2(initializationValues);
            },
            function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        } else {
            initializeBlade2(dynPropertyValues);
        }
    };

    function initializeBlade2(data) {
        blade.currentEntities = angular.copy(data);
        blade.origEntity = data;
        blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals(blade.currentEntities, blade.origEntity);
    }

    $scope.cancelChanges = function () {
        angular.copy(blade.origEntity, blade.currentEntities);
        $scope.bladeClose();
    };

    $scope.saveChanges = function () {
        var valuesToSave = _.filter(blade.currentEntities, function (x) { return !x.property.isMultilingual; });;
        var selectedProps = _.filter(blade.currentEntities, function (x) { return x.property.isMultilingual; });
        _.each(selectedProps, function (prop) {
            _.each(prop.values, function (value) {
                if (value.values.length > 0 && value.values[0].value) {
                    valuesToSave.push({ property: prop.property, locale: value.locale, values: _.pluck(value.values, 'value') });
                }
            });
        });

        blade.currentEntities = valuesToSave;
        angular.copy(valuesToSave, blade.origEntity);
        angular.copy(valuesToSave, blade.data);
        $scope.bladeClose();
    };

    blade.onClose = function (closeCallback) {
        closeChildrenBlades();

        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The properties has been modified. Do you want to save changes?",
                callback: function (needSave) {
                    if (needSave) {
                        $scope.saveChanges();
                    }
                    closeCallback();
                }
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    function closeChildrenBlades() {
        angular.forEach(blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.editProperty = function (node) {
        var newBlade = {
            id: "dynamicPropertyDetail",
            title: node.property.objectType,
            subtitle: 'Manage property',
            origEntity: node,
            confirmChangesFn: function (entry) {
                angular.copy(entry, node);
                $scope.saveChanges();
            },
            deleteFn: function () {
                //var idx = blade.currentEntities.indexOf(node);
                //if (idx >= 0) {
                //    blade.currentEntities.splice(idx, 1);
                //}
                dynamicPropertiesApi.delete({ id: blade.currentEntityId, propertyId: node.id },
                    blade.refresh,
                    function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            },
            controller: 'platformWebApp.dynamicPropertyDetailController',
            template: 'Scripts/app/dynamicProperties/blades/dynamicProperty-detail.tpl.html'
            //controller: 'virtoCommerce.customerModule.memberPropertyDetailController',
            //template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/member-property-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    blade.toolbarCommands = [
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntities);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        }
    ];


    $scope.$watch('blade.parentBlade.currentEntity.dynamicPropertyValues', initializeBlade);
}]);
