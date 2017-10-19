angular.module('platformWebApp')
    .filter('boolToValue', function () {
        return function (input, trueValue, falseValue) {
            return input ? trueValue : falseValue;
        };
    })
    .filter('slice', function () {
        return function (arr, start, end) {
            return (arr || []).slice(start, end);
        };
    })
    .filter('readablesize', function () {
        return function (input) {
            if (!input)
                return null;

            var sizes = ["Bytes", "KB", "MB", "GB", "TB"];
            var order = 0;
            while (input >= 1024 && order + 1 < sizes.length) {
                order++;
                input = input / 1024;
            }
            return Math.round(input) + ' ' + sizes[order];
        };
    })
    // translate the given properties in the input array
    .filter('translateArray', ['$translate', function ($translate) {
        return function (inputArray, propertiesList) {
            _.each(inputArray, function (inputItem) {
                _.each(propertiesList, function (prop) {
                    if (angular.isString(inputItem[prop])) {
                        var translateKey = inputItem[prop].toLowerCase();
                        var result = $translate.instant(translateKey);
                        if (result !== translateKey) inputItem[prop] = result;
                    }
                });
            });
            return inputArray;
        }
    }])
    // translation with fall-back value if key not found
    .filter('fallbackTranslate', ['$translate', function ($translate) {
        return function (translateKey, fallbackValue) {
            var result = $translate.instant(translateKey);
            return result === translateKey ? fallbackValue : result;
        };
    }]);