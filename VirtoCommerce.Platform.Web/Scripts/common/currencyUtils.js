angular.module('platformWebApp')
.factory('platformWebApp.currencyUtils', ['platformWebApp.numberUtils', '$filter', '$locale', function (numberUtils, $filter, $locale) {
    var getPattern = function() {
        var pattern = $locale.NUMBER_FORMATS.PATTERNS[1];
        angular.forEach(pattern, function (patternPart, patternPartKey) {
            if (angular.isString(patternPart)) {
                patternPart = patternPart.replace(/\s*¤\s*/g, "");
                pattern[patternPartKey] = patternPart;
            }
        });
        return pattern;
    };

    // Service provide variables and functions for number (float & integer) convertion and validation
    var result = {
        validate: function (viewValue, minExclusive, min, maxExclusive, max, fraction, setValidity) {
            var negativeMatches;
            var value;
            var pattern = getPattern();
            var vars = numberUtils.getVariables(pattern, minExclusive, min, maxExclusive, max, fraction || pattern.maxFrac);
            var regexp = new RegExp(vars.regexpFloat);
            var isValid = regexp.test(viewValue);
            if (isValid) {
                negativeMatches = viewValue.match(vars.negativeRegExp);
                value = parseFloat(viewValue
                    .replace(negativeMatches && negativeMatches.length > 1 ? vars.negativePrefix : null, '-')
                    .replace(vars.positivePrefix, '')
                    .replace(new RegExp(vars.escapedGroupSeparator, "g"), '')
                    .replace(vars.decimalSeparator, '.')
                    .replace(vars.negativeSuffix, '')
                    .replace(vars.positiveSuffix, ''));
                if (setValidity)
                    setValidity('number', true);
                return numberUtils.inRange(value, minExclusive, min, maxExclusive, max, setValidity);
            }
            if (setValidity)
                setValidity('number', false);
            return undefined;
        },
        format: function (modelValue, minExclusive, min, maxExclusive, max, fraction) {
            var pattern = getPattern();
            return $filter('currency')(parseFloat(modelValue), '', numberUtils.getVariables(pattern, minExclusive, min, maxExclusive, max, fraction || pattern.maxFrac).maximumFraction);
        }
    };
    return result;
}]);