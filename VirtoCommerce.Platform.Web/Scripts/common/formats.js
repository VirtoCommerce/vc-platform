angular.module('platformWebApp')
// Service provide functions for currency convertion and validation with localization support
.factory('platformWebApp.currencyFormat', ['platformWebApp.numberFormat', '$filter', '$locale', function (numberFormat, $filter, $locale) {
    // Remove currency symbol from validation regular expression: we want only value in input
    var getPattern = function () {
        var pattern = $locale.NUMBER_FORMATS.PATTERNS[1];
        angular.forEach(pattern, function (patternPart, patternPartKey) {
            if (angular.isString(patternPart)) {
                patternPart = patternPart.replace(/\s*¤\s*/g, "");
                pattern[patternPartKey] = patternPart;
            }
        });
        return pattern;
    };

    var result = {
        validate: function (viewValue, minExclusive, min, maxExclusive, max, fraction, setValidity) {
            var negativeMatches;
            var value;
            var pattern = getPattern();
            var vars = numberFormat.getVariables(pattern, minExclusive, min, maxExclusive, max, fraction || pattern.maxFrac);
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
                return numberFormat.inRange(value, minExclusive, min, maxExclusive, max, setValidity);
            }
            if (setValidity)
                setValidity('number', false);
            return undefined;
        },
        format: function (modelValue, minExclusive, min, maxExclusive, max, fraction) {
            var pattern = getPattern();
            return $filter('currency')(parseFloat(modelValue), '', numberFormat.getVariables(pattern, minExclusive, min, maxExclusive, max, fraction || pattern.maxFrac).maximumFraction);
        }
    };
    return result;
}])
// Service provide variables and functions for number (float & integer) convertion and validation with localization support
.factory('platformWebApp.numberFormat', ['$filter', '$locale', function ($filter, $locale) {
    var escape = function (str) {
        // escape string, replace any explicit spaces with spaces group
        return RegExp.escape(str).replace(/\s/g, "\\s");
    };
    var formatRegExp = function (str) {
        // \u221e = ∞ (infinity)
        return "^(" + str + "|\u221e)$";
    };
    var result = {
        // based on https://github.com/angular/angular.js/blob/e812b9fa9ec7086ab8d64a32d86f6e991f84bc55/src/ng/filter/filters.js#L289
        getVariables: function (pattern, minExclusive, min, maxExclusive, max, fraction) {
            var result = {};

            result.isNegativeAllowed = angular.isDefined(minExclusive) ? minExclusive <= 0 : angular.isDefined(min) ? min < 0 : true;
            result.isPositiveAllowed = angular.isDefined(maxExclusive) ? maxExclusive >= 0 : angular.isDefined(max) ? max > 0 : true;

            // Negative (like "-") and positive (like "+") prefixies
            result.negativePrefix = pattern.negPre;
            result.positivePrefix = pattern.posPre;
            result.isPrefixExists = result.negativePrefix == result.positivePrefix == "";
            result.escapedNegativePrefix = escape(result.negativePrefix);
            result.escapedPositivePrefix = escape(result.positivePrefix);

            // Group separator, i.e. 111,111 or 111 111
            result.escapedGroupSeparator = escape($locale.NUMBER_FORMATS.GROUP_SEP);
            result.groupSize = pattern.gSize;
            result.lastGroupSize = pattern.lgSize;

            // Decimal separator, i.e. 111.00 or 111,00
            result.decimalSeparator = $locale.NUMBER_FORMATS.DECIMAL_SEP;
            result.escapedDecimalSeparator = escape(result.decimalSeparator);
            result.minimalFraction = fraction || pattern.minFrac;
            result.maximumFraction = fraction || pattern.maxFrac || 21; // ECMA 262

            // Negative (like "-") and positive (like "+") suffixies
            result.negativeSuffix = pattern.negSuf;
            result.positiveSuffix = pattern.posSuf;
            result.isSuffixExists = result.negativeSuffix == result.positiveSuffix == "";
            result.escapedNegativeSuffix = escape(result.negativeSuffix);
            result.escapedPositiveSuffix = escape(result.positiveSuffix);

            // Select prefexies, like [-+]
            result.regexpPrefix = (result.isPrefixExists ? "(" + (result.isNegativeAllowed ? result.escapedNegativePrefix : "") + "|" + (result.isPositiveAllowed ? result.escapedPositivePrefix : "") + ")" : "");

            // Select value. There is three groups: start (without leading zeroes), middle (size of both defined in groupSize) and end (size defined in lastGroupSize)
            var startGroup = "([1-9][0-9]{0," + (result.groupSize > 0 ? result.groupSize - 1 : 0) + "}" + ")";
            var middleGroup = "(" + result.escapedGroupSeparator + "\\d{" + result.groupSize + "}" + ")";
            var endGroup = "(" + result.escapedGroupSeparator + "\\d{" + result.lastGroupSize + "})";
            // Number may have or start group (sss), start group + last group (sss,lll) or start group, multiple middle groups and end group (sss,mmm,mmm,lll)
            result.regexpValue = "((" + startGroup + ("(" + middleGroup + endGroup + "|" + endGroup + ")?") + "|[1-9][0-9]*)|0)";

            // Select fraction. May not exists and have maximum size defined by maximumFraction.
            // Minimum size ignored - we want to allow use type just 111 instead of 111.000
            result.regexpFraction = "(" + result.escapedDecimalSeparator + "\\d{1," + result.maximumFraction + "}" + ")?";

            // Select suffixies, like [-+]. This required because, for example, in some locales negative numbers may be defined as (111) instead of -111
            result.regexpSuffix = (result.isSuffixExists ? "[" + (result.isNegativeAllowed ? result.escapedNegativeSuffix : "") + (result.isPositiveAllowed ? result.escapedPositiveSuffix : "") + "]?" : "");

            result.regexpFloat = formatRegExp(result.regexpPrefix + result.regexpValue + result.regexpFraction + result.regexpSuffix);
            result.regexpInteger = formatRegExp(result.regexpPrefix + result.regexpValue + result.regexpSuffix);

            // Check is value a negative. Be careful! This regular expression does not check value for validity (see above), only for negativity
            result.negativeRegExp = new RegExp("^(" + result.escapedNegativePrefix + ")[^" + result.escapedNegativePrefix + result.escapedNegativeSuffix + "](" + result.escapedNegativeSuffix + ")$");

            return result;
        },
        // Check if number in specified range
        inRange: function (value, minExclusive, min, maxExclusive, max, setValidity) {
            var notLessThanMin = true;
            var notGreaterThanMax = true;
            if (minExclusive || min) {
                notLessThanMin = minExclusive < value || min <= value;
                if (setValidity)
                    setValidity('min', notLessThanMin);
            }
            if (maxExclusive || max) {
                notGreaterThanMax = value < maxExclusive || value <= max;
                if (setValidity)
                    setValidity('max', notGreaterThanMax);
            }
            if (!notLessThanMin || !notGreaterThanMax) {
                return undefined;
            }
            return value;
        },
        validate: function (viewValue, numType, minExclusive, min, maxExclusive, max, fraction, setValidity) {
            var vars;
            var regexp;
            var isValid;
            var negativeMatches;
            var value;
            if (numType === "float") {
                vars = this.getVariables($locale.NUMBER_FORMATS.PATTERNS[0], minExclusive, min, maxExclusive, max, fraction);
                regexp = new RegExp(vars.regexpFloat);
                isValid = regexp.test(viewValue);
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
                        setValidity('float', true);
                    return this.inRange(value, minExclusive, min, maxExclusive, max, setValidity);
                }
                if (setValidity)
                    setValidity('float', false);
                return undefined;
            } else {
                vars = this.getVariables($locale.NUMBER_FORMATS.PATTERNS[0]);
                regexp = new RegExp(vars.regexpInteger);
                isValid = regexp.test(viewValue);
                if (isValid) {
                    negativeMatches = viewValue.match(vars.negativeRegExp);
                    value = parseFloat(viewValue
                        .replace(negativeMatches && negativeMatches.length > 1 ? vars.negativePrefix : null, '-')
                        .replace(vars.positivePrefix, '')
                        .replace(new RegExp(vars.escapedGroupSeparator, "g"), '')
                        .replace(vars.negativeSuffix, '')
                        .replace(vars.positiveSuffix, ''));
                    if (setValidity)
                        setValidity('integer', true);
                    return this.inRange(value, minExclusive, min, maxExclusive, max, setValidity);
                }
                if (setValidity)
                    setValidity('integer', false);
                return undefined;
            }
        },
        format: function (modelValue, numType, minExclusive, min, maxExclusive, max, fraction) {
            if (numType === "float") {
                return $filter('number')(parseFloat(modelValue), this.getVariables($locale.NUMBER_FORMATS.PATTERNS[0], minExclusive, min, maxExclusive, max, fraction).maximumFraction);
            } else {
                return $filter('number')(parseFloat(modelValue), 0);
            }
        }
    };
    return result;
}])
// Service provide variables and functions for angular to moment convertion and validation
.factory('platformWebApp.angularToMomentFormatConverter', ['$locale', '$log', function ($locale, $log) {
    var result = {
        pairs: [
            { 'yyyy': 'YYYY' }, { 'yy': 'YY' }, { 'y': 'Y' },
            { 'dd': 'DD' }, { 'd': 'D' },
            { 'eee': 'ddd' }, { 'EEEE': 'dddd' },
            { 'sss': 'SSS' },
            { 'Z': 'ZZ' }
        ],
        valid: ["yyyy', 'yy', 'y', 'MMMM', 'MMM', 'MM', 'M', 'dd', 'd', 'EEEE', 'EEE', , 'ww', 'w"],
        invalid: ['G', 'GG', 'GGG', 'GGGG'],
        validDate: ["yyyy', 'yy', 'y', 'MMMM', 'MMM', 'MM', 'M', 'dd', 'd', 'EEEE', 'EEE', 'ww', 'w"],
        validTime: ['HH', 'H', 'hh', 'h', 'mm', 'm', 'ss', 's', 'sss', 'a', 'Z'],
        additionalFormats: ['medium', 'short', 'mediumTime', 'shortTime', 'fullDate', 'longDate', 'mediumDate', 'shortDate'],
        additionalDateFormats: ['fullDate', 'longDate', 'mediumDate', 'shortDate'],
        additionalTimeFormats: ['mediumTime', 'shortTime'],
        additionalMixedFormats: ['medium', 'short'],
        isInvalid: function (originalFormat) {
            return !this.additionalFormats.includes(originalFormat) && !this.invalid.includes(originalFormat);
        },
        isInvalidDate: function (originalFormat) {
            return _.every([this.additionalTimeFormats, this.additionalMixedFormats, this.validTime, this.invalid].forEach(function (formats) {
                return _.every(formats, function (format) {
                    return originalFormat.indexOf(format) < 0;
                });
            }));
        },
        isInvalidTime: function (originalFormat) {
            return _.every([this.additionalDateFormats, this.additionalMixedFormats, this.validDate, this.invalid].forEach(function (formats) {
                return _.all(formats, function (format) {
                    return originalFormat.indexOf(format) < 0;
                });
            }));
        },
        isInvalidMixed: function (originalFormat) {
            var result = _.every([this.additionalTimeFormats, this.additionalDateFormats, this.invalid].forEach(function (formats) {
                return _.every(formats, function (format) {
                    return originalFormat.indexOf(format) < 0;
                });
            }));
            if (result) {
                result |= !_.some(this.additionalMixedFormas, function (mixedFormat) {
                    return originalFormat.indexOf(mixedFormat) >= 0;
                });
                if (result) {
                    result |= _.every([
                        _.some(this.validDate, function (format) {
                            return originalFormat.indexOf(format) >= 0;
                        }), _.some(this.validTime, function (format) {
                            return originalFormat.indexOf(format) >= 0;
                        })
                    ]);
                    return result;
                }
            }
            return false;
        },
        validate: function (originalFormat, validator) {
            var formatParts = originalFormat.split("'");
            if (_.some(formatParts.forEach(function (formatPart) {
                return validator(formatPart);
            }))) {
                $log.error("Invalid date format");
                return false;
            }
            return true;
        },
        convert: function (originalFormat) {
            var format = originalFormat;
            if (this.additionalFormats.includes(format)) {
                format = $locale.DATETIME_FORMATS[format];
            }
            var formatParts = format.split("'");
            for (var i = 0; i < formatParts.length; i++) {
                if (!_.every(this.invalid, function (token) { return formatParts.indexOf(token) < 0 })) {
                    $log.error("Moment doesn't support one of used formats");
                    return undefined;
                }
                var replace = function (formatPart) {
                    this.pairs.forEach(function (token) {
                        var key = Object.keys(token)[0];
                        formatPart = formatPart.replace(new RegExp(RegExp.escape(key), "g"), token[key]);
                    });
                    return formatPart;
                };
                formatParts[i] = replace.apply(this, [formatParts[i]]);
            }
            return formatParts.join('');
        }
    };
    return result;
}]);