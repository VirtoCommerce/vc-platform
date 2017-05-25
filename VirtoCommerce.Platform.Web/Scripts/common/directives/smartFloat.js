'use strict';

angular.module('platformWebApp')
.directive('smartFloat', [
    '$filter', '$compile', '$locale', function ($filter, $compile, $locale) {

        // https://stackoverflow.com/questions/3446170/escape-string-for-use-in-javascript-regex
        function escapeRegExp(str) {
            return str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&").replace(/\s/g, " ");
        }

        function formatRegExp(str) {
            return "^" + str + "$";
        }

        return {
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                var validationVariables = function () {
                    var result = { };

                    result["isNegativeAllowed"] = !(attrs.min && attrs.min >= 0);
                    result["isPositiveAllowed"] = !(attrs.max && attrs.max <= 0);

                    result["negativePrefix"] = $locale.NUMBER_FORMATS.PATTERNS[0].negPre;
                    result["positivePrefix"] = $locale.NUMBER_FORMATS.PATTERNS[0].posPre;
                    result["isPrefixExists"] = result.negativePrefix == result.positivePrefix == "";
                    result["escapedNegativePrefix"] = escapeRegExp(result.negativePrefix);
                    result["escapedPositivePrefix"] = escapeRegExp(result.positivePrefix);

                    result["escapedGroupSeparator"] = escapeRegExp($locale.NUMBER_FORMATS.GROUP_SEP);
                    result["groupSize"] = $locale.NUMBER_FORMATS.PATTERNS[0].gSize;
                    result["lastGroupSize"] = $locale.NUMBER_FORMATS.PATTERNS[0].lgSize;

                    result["decimalSeparator"] = $locale.NUMBER_FORMATS.DECIMAL_SEP;
                    result["escapedDecimalSeparator"] = escapeRegExp(result.decimalSeparator);
                    result["minimalFraction"] = attrs.fraction ? attrs.fraction : $locale.NUMBER_FORMATS.PATTERNS[0].minFrac;
                    result["maximumFraction"] = attrs.fraction ? attrs.fraction : $locale.NUMBER_FORMATS.PATTERNS[0].maxFrac;

                    result["negativeSuffix"] = $locale.NUMBER_FORMATS.PATTERNS[0].negSuf;
                    result["positiveSuffix"] = $locale.NUMBER_FORMATS.PATTERNS[0].posSuf;
                    result["isSuffixExists"] = result.negativeSuffix == result.positiveSuffix == "";
                    result["escapedNegativeSuffix"] = escapeRegExp(result.negativeSuffix);
                    result["escapedPositiveSuffix"] = escapeRegExp(result.positiveSuffix);

                    result["regexpPrefix"] = (result.isPrefixExists ? "[" + (result.isNegativeAllowed ? result.escapedNegativePrefix : "") + (result.isPositiveAllowed ? result.escapedPositivePrefix : "") + "]?" : "");
                    result["regexpValue"] = "(((\\d{1," + result.groupSize + "}" + result.escapedGroupSeparator + ")?" + "(\\d{" + result.groupSize + "}" + result.escapedGroupSeparator + ")*" + "(\\d{1," + result.lastGroupSize + "}))|\\d*)";
                    result["regexpZero"] = "0";
                    result["regexpFraction"] = "(" + result.escapedDecimalSeparator + "\\d{" + result.minimalFraction + 1 + "," + result.maximumFraction + "}" + ")" + (result.minimalFraction === 0 ? "?" : "");
                    result["regexpZeroFraction"] = result.regexpFraction.replace("\\d", "0");
                    result["regexpSuffix"] = (result.isSuffixExists ? "[" + (result.isNegativeAllowed ? result.escapedNegativeSuffix : "") + (result.isPositiveAllowed ? result.escapedPositiveSuffix : "") + "]?" : "");

                    result["negativeRegExp"] = new RegExp("^(" + result.escapedNegativePrefix + ")[^" + result.escapedNegativePrefix + result.escapedNegativeSuffix + "](" + result.escapedNegativeSuffix + ")$");

                    return result;
                };

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
                        var vars = validationVariables();
                        var regexp = new RegExp(formatRegExp(vars.regexpPrefix + vars.regexpValue + vars.regexpFraction + vars.regexpSuffix));
                        var isValid = regexp.test(viewValue);
                        if (!isValid) {
                            regexp = new RegExp(formatRegExp(vars.regexpZero + vars.regexpZeroFraction));
                            isValid = regexp.test(viewValue);
                        }
                        if (isValid) {
                            var negativeMatches = viewValue.match(vars.negativeRegExp);
                            var value = parseFloat(viewValue
                                .replace(negativeMatches && negativeMatches.length > 1 ? vars.negativePrefix : null, '-')
                                .replace(vars.positivePrefix, '')
                                .replace(new RegExp(vars.escapedGroupSeparator, "g"), '')
                                .replace(vars.decimalSeparator, '.')
                                .replace(vars.negativeSuffix, '')
                                .replace(vars.positiveSuffix, ''));
                            ctrl.$setValidity('float', true);
                            return inRange(value, attrs.min, attrs.max);
                        }
                        ctrl.$setValidity('float', false);
                        return undefined;
                    });

                    ctrl.$formatters.unshift(
                        function(modelValue) {
                            return $filter('number')(parseFloat(modelValue), validationVariables().maximumFraction);
                        }
                    );
                } else {
                    ctrl.$parsers.unshift(function (viewValue) {
                        var vars = validationVariables();
                        var regexp = new RegExp(formatRegExp(vars.regexpPrefix + vars.regexpValue + vars.regexpSuffix));
                        var isValid = regexp.test(viewValue);
                        if (!isValid) {
                            regexp = new RegExp(formatRegExp(vars.regexpZero));
                            isValid = regexp.test(viewValue);
                        }
                        if (isValid) {
                            var negativeMatches = viewValue.match(vars.negativeRegExp);
                            var value = parseFloat(viewValue
                                .replace(negativeMatches && negativeMatches.length > 1 ? vars.negativePrefix : null, '-')
                                .replace(vars.positivePrefix, '')
                                .replace(new RegExp(vars.escapedGroupSeparator, "g"), '')
                                .replace(vars.negativeSuffix, '')
                                .replace(vars.positiveSuffix, ''));
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

                scope.$on('$localeChangeSuccess', function () {
                    ctrl.$viewValue = ctrl.$formatters.reduceRight((prev, fn) => fn(prev), ctrl.$modelValue);
                    ctrl.$render();
                });
            }
        };
    }
]);