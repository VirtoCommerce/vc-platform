angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.propertyDictionaryController', ['$scope', '$filter', 'platformWebApp.dialogService', function ($scope, $filter, dialogService) {
    var dictionaryValues;
    var pb = $scope.blade.parentBlade;
    $scope.pb = pb;

    $scope.dictValueValidator = function (value) {
        return _.all(dictionaryValues, function (item) { return item.value !== value; });
    }

    $scope.cancel = function () {
        $scope.selectedItem = undefined;
        resetNewValue();
    }

    $scope.add = function (form) {
        if (form.$valid) {
            if ($scope.newValue.values) {
                if ($scope.selectedItem) { // editing existing values
                    _.each($scope.newValue.values, function (value) {
                        var existingValue = _.findWhere(dictionaryValues, { alias: value.alias, languageCode: value.languageCode });
                        if (value.value) {
                            if (existingValue) {
                                existingValue.alias = $scope.newValue.values[0].value;
                                existingValue.value = value.value;
                            } else {
                                dictionaryValues.push({
                                    alias: $scope.newValue.values[0].value,
                                    languageCode: value.languageCode,
                                    propertyId: pb.currentEntity.id,
                                    value: value.value
                                });
                            }
                        } else if (existingValue) {
                            $scope.delete(dictionaryValues.indexOf(existingValue));
                        }
                    });
                    $scope.selectedItem = undefined;
                } else { // adding new values
                    _.each($scope.newValue.values, function (value) {
                        if (value.value) {
                            dictionaryValues.push({
                                alias: $scope.newValue.values[0].value,
                                languageCode: value.languageCode,
                                propertyId: pb.currentEntity.id,
                                value: value.value
                            });
                        }
                    });
                }
                initializeDictionaryValues(dictionaryValues);
            } else {
                dictionaryValues.push($scope.newValue);
            }
            resetNewValue($scope.newValue.languageCode);
            form.$setPristine();
        }
    };

    $scope.selectItem = function (listItem) {
        $scope.selectedItem = listItem;

        if (pb.currentEntity.multilanguage) {
            resetNewValue();
        }
    };

    $scope.delete = function (index) {
        dictionaryValues.splice(index, 1);
        $scope.selectedItem = undefined;
    };
    $scope.deleteMultilanguage = function (key) {
        var selectedValues = _.where(dictionaryValues, { alias: key });
        _.forEach(selectedValues, function (value) {
            dictionaryValues.splice(dictionaryValues.indexOf(value), 1);
        });
        initializeDictionaryValues(dictionaryValues);
        $scope.selectedItem = undefined;
    };

    $scope.blade.toolbarCommands = [
     {
         name: "Delete", icon: 'fa fa-trash-o',
         executeMethod: function () {
             deleteChecked();
         },
         canExecuteMethod: function () {
             return isItemsChecked();
         }
     }
    ];

    $scope.checkAll = function (selected) {
        angular.forEach(getValuesList(), function (item) {
            item.selected = selected;
        });
    };

    function getValuesList() {
        return pb.currentEntity.multilanguage ? $scope.groupedValues : dictionaryValues;
    }

    function resetNewValue(locale) {
        if (pb.currentEntity.multilanguage) {
            // generate input fields for ALL languages
            var defaultLanguageCode = pb.currentEntity.catalog.defaultLanguage.languageCode;
            var values = [{ languageCode: defaultLanguageCode }];
            _.each(pb.currentEntity.catalog.languages, function (lang) {
                if (lang.languageCode !== defaultLanguageCode) {
                    values.push({ languageCode: lang.languageCode });
                }
            });

            // add current values
            if ($scope.selectedItem) {
                _.each($scope.selectedItem.values, function (value) {
                    var foundValue = _.findWhere(values, { languageCode: value.languageCode });
                    if (foundValue) {
                        angular.extend(foundValue, value);
                    }
                });
            }

            $scope.newValue = { values: values };
        } else {
            $scope.newValue = { languageCode: locale, value: null, propertyId: pb.currentEntity.id };
        }
    }

    function isItemsChecked() {
        return _.any(getValuesList(), function (x) { return x.selected; });
    }

    function deleteChecked() {
        var dialog = {
            id: "confirmDeleteItem",
            title: "Delete confirmation",
            message: "Are you sure you want to delete selected dictionary values?",
            callback: function (remove) {
                if (remove) {
                    var selection = $filter('filter')(getValuesList(), { selected: true }, true);
                    angular.forEach(selection, function (listItem) {
                        if (pb.currentEntity.multilanguage) {
                            $scope.deleteMultilanguage(listItem.alias);
                        } else {
                            $scope.delete(getValuesList().indexOf(listItem));
                        }
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    function initializeDictionaryValues(data) {
        dictionaryValues = data;
        $scope.dictionaryValues = data;
        $scope.groupedValues = _.map(_.groupBy(data, 'alias'), function (values, key) {
            return { alias: key, values: values };
        });
    }

    $scope.$watch('blade.parentBlade.currentEntity.dictionaryValues', initializeDictionaryValues);

    // on load
    resetNewValue(pb.currentEntity.catalog.defaultLanguage.languageCode);
    $scope.blade.isLoading = false;
}]);
