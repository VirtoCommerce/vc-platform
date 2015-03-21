angular.module('platformWebApp')
    .filter('boolToValue', function () {
        return function (input, trueValue, falseValue) {
            return input ? trueValue : falseValue;
        };
    }).filter('slice', function ()
    {
        return function (arr, start, end)
        {
            return (arr || []).slice(start, end);
        };
    });;