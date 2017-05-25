'use strict';

angular.module('platformWebApp')
.directive('smartFloat', [
    '$filter', '$compile', '$locale', function ($filter, $compile, $locale) {

        // https://stackoverflow.com/questions/3446170/escape-string-for-use-in-javascript-regex
        function escapeRegExp(str) {
            return str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&").replace(/\s/, " ");
        }

        function formatRegExp(str) {
            return "^" + str + "$";
        }

        return {
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                var isNegativeAllowed = !(attrs.min && attrs.min >= 0);
                var isPositiveAllowed = !(attrs.max && attrs.max <= 0);

                var negativePrefix = isNegativeAllowed ? $locale.NUMBER_FORMATS.PATTERNS[0].negPre : "";
                var positivePrefix = isPositiveAllowed ? $locale.NUMBER_FORMATS.PATTERNS[0].posPre : "";
                var isPrefixExists = negativePrefix == positivePrefix == "";
                var escapedNegativePrefix = escapeRegExp(negativePrefix);
                var escapedPositivePrefix = escapeRegExp(positivePrefix);

                var escapedGroupSeparator = escapeRegExp($locale.NUMBER_FORMATS.GROUP_SEP);
                var groupSize = $locale.NUMBER_FORMATS.PATTERNS[0].gSize;
                var largeGroupSize = $locale.NUMBER_FORMATS.PATTERNS[0].lgSize;

                var decimalSeparator = $locale.NUMBER_FORMATS.DECIMAL_SEP;
                var escapedDecimalSeparator = escapeRegExp(decimalSeparator);
                var minimalFraction = attrs.fraction ? attrs.fraction : $locale.NUMBER_FORMATS.PATTERNS[0].minFrac;
                var maximumFraction = attrs.fraction ? attrs.fraction : $locale.NUMBER_FORMATS.PATTERNS[0].maxFrac;

                var negativeSuffix = isNegativeAllowed ? $locale.NUMBER_FORMATS.PATTERNS[0].negSuf : "";
                var positiveSuffix = isPositiveAllowed ? $locale.NUMBER_FORMATS.PATTERNS[0].posSuf : "";
                var isSuffixExists = negativeSuffix == positiveSuffix == "";
                var escapedNegativeSuffix = escapeRegExp(negativeSuffix);
                var escapedPositiveSuffix = escapeRegExp(positiveSuffix);

                var regexpPrefix = (isPrefixExists ? "[" + escapedNegativePrefix + escapedPositivePrefix + "]?" : "");
                var regexpValue = "(((\\d{1," + groupSize + "}" + escapedGroupSeparator + ")?" + "(\\d{" + groupSize + "}" + escapedGroupSeparator + ")*" + "(\\d{1," + largeGroupSize + "}))|\\d*)";
                var regexpZero = "0";
                var regexpFraction = "(" + escapedDecimalSeparator + "\\d{" + minimalFraction + 1 + "," + maximumFraction + "}" + ")" + (minimalFraction === 0 ? "?" : "");
                var regexpZeroFraction = regexpFraction.replace("\\d", "0");
                var regexpSuffix = (isSuffixExists ? "[" + escapedNegativeSuffix + escapedPositiveSuffix + "]?" : "");

                var negativeRegExp = new RegExp("^(" + escapedNegativePrefix + ")[^" + escapedNegativePrefix + escapedNegativeSuffix + "](" + escapedNegativeSuffix + ")$");

                var inRange = function(value, min, max)
                {
                    var isGreaterThanOrEqualToMin = true;
                    var isLessThanOrEqualToMax = true;
                    if (min) {
                        isGreaterThanOrEqualToMin = min <= value;
                        ctrl.$setValidity('min', isGreaterThanOrEqualToMin);
                    }
                    if (max) {
                        isLessThanOrEqualToMax = value <= max;
                        ctrl.$setValidity('max', isLessThanOrEqualToMax);
                    }
                    if (!isGreaterThanOrEqualToMin || !isLessThanOrEqualToMax) {
                        return undefined;
                    }
                    return value;
                }

                if (attrs.numType === "float") {
                    ctrl.$parsers.unshift(function (viewValue) {
                        var regexp = new RegExp(formatRegExp(regexpPrefix + regexpValue + regexpFraction + regexpSuffix));
                        var isValid = regexp.test(viewValue);
                        if (!isValid) {
                            regexp = new RegExp(formatRegExp(regexpZero + regexpZeroFraction));
                            isValid = regexp.test(viewValue);
                        }
                        if (isValid) {
                            var negativeMatches = viewValue.match(negativeRegExp);
                            var value = parseFloat(viewValue
                                .replace(negativeMatches && negativeMatches.length > 1 ? negativePrefix : '-', '')
                                .replace(positivePrefix, '')
                                .replace(new RegExp(escapedGroupSeparator, "g"), '')
                                .replace(decimalSeparator, '.')
                                .replace(negativeSuffix, '')
                                .replace(positiveSuffix, ''));
                            ctrl.$setValidity('float', true);
                            return inRange(value, attrs.min, attrs.max);
                        }
                        ctrl.$setValidity('float', false);
                        return undefined;
                    });

                    ctrl.$formatters.unshift(
                        function(modelValue) {
                            return $filter('number')(parseFloat(modelValue), maximumFraction);
                        }
                    );
                } else {
                    ctrl.$parsers.unshift(function (viewValue) {
                        var regexp = new RegExp(formatRegExp(regexpPrefix + regexpValue + regexpSuffix));
                        var isValid = regexp.test(viewValue);
                        if (!isValid) {
                            regexp = new RegExp(formatRegExp(regexpZero));
                            isValid = regexp.test(viewValue);
                        }
                        if (isValid) {
                            var negativeMatches = viewValue.match(negativeRegExp);
                            var value = parseFloat(viewValue
                                .replace(negativeMatches && negativeMatches.length > 1 ? negativePrefix : '', '-')
                                .replace(positivePrefix, '')
                                .replace(new RegExp(escapedGroupSeparator, "g"), '')
                                .replace(negativeSuffix, '')
                                .replace(positiveSuffix, ''));
                            ctrl.$setValidity('integer', true);
                            return inRange(value, attrs.min, attrs.max);
                        }
                        ctrl.$setValidity('integer', false);
                        return undefined;
                    });


                    ctrl.$formatters.unshift(
                        function (modelValue) {
                            return $filter('number')(parseFloat(modelValue), 0);
                        }
                    );
                }
            }
        };
    }
]);