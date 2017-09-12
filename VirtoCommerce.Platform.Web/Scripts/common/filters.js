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
    }).filter('readablesize', function() {
        return function (input) {
            if (!input)
                return null;
             
            var sizes = [ "Bytes", "KB", "MB", "GB", "TB" ];
            var order = 0;
            while (input >= 1024 && order + 1 < sizes.length)
            {
                order++;
                input = input / 1024;
            }
            return Math.round(input) + ' ' + sizes[order];
        };
    });;