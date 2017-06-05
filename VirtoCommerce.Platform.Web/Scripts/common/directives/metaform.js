angular.module('platformWebApp')
.factory('platformWebApp.metaFormsService', [function () {
    var metaFormCallbacks = { };
    var metaFields = { };

    return {
        registerMetaFields: function (name, fields) {
            var oldMetaFields = metaFields[name];
            metaFields[name] = (metaFields[name] || []).concat(fields);
            var newMetaFileds = metaFields[name];
            if (metaFormCallbacks[name]) {
                metaFormCallbacks[name].forEach(function(callback) {
                    callback(newMetaFileds, oldMetaFields);
                });
            }
        },
        getMetaFields: function(name) {
            return metaFields[name];
        },
        onMetaFieldsUpdate(name, callback) {
            (metaFormCallbacks[name] = metaFormCallbacks[name] || []).push(callback);
        }
    };
}])
.directive('vaMetaform', [function () {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            blade: '=',
            registeredInputs: '=',
            columnCount: '@?'
        },
        templateUrl: '$(Platform)/Scripts/common/directives/metaform.tpl.html',
        link: function (scope) {
            var columnCount = scope.columnCount ? parseInt(scope.columnCount, 10) : 1;

            var filteredInputs = _.filter(scope.registeredInputs, function (x) { return !x.isVisibleFn || x.isVisibleFn(scope.blade); });

            var resultingGroups = [];
            var isGroupStart = true, currentGroup;
            _.each(filteredInputs, function (x) {
                x = angular.copy(x);
                x.ngBindingModel = x.name;

                if (isGroupStart || (x.templateUrl && x.spanAllColumns)) {
                    currentGroup = [x];
                    resultingGroups.push(currentGroup);
                } else {
                    currentGroup.push(x);
                }
                isGroupStart = (x.templateUrl && x.spanAllColumns) || currentGroup.length == columnCount;
            });

            // generate empty columns
            _.each(resultingGroups, function (gr) {
                while (columnCount > gr.length) {
                    gr.push({ templateUrl: 'emptyColumn.html' });
                }
            });
            scope.bladeInputGroups = resultingGroups;
        }
    }
}]);