'use strict';

angular.module('platformWebApp')
.directive('smartFloat', [
    '$filter', '$compile', '$locale', function ($filter, $compile, $locale) {

        // https://stackoverflow.com/questions/3446170/escape-string-for-use-in-javascript-regex
        function escapeRegExp(str) {
            return str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&");
        }

        function formatRegExp(str) {
            return "^" + str + "$";
        }

        function inRange(value, min, max) {
            return (!min || value < min) && (!max || value > max);
        }

        return {
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                var isNegativeAllowed = !(attrs.min && attrs.min >= 0);
                var isPositiveAllowed = !(attrs.max && attrs.max <= 0);

                var negPre = isNegativeAllowed ? $locale.NUMBER_FORMATS.PATTERNS[0].negPre : "";
                var posPre = isPositiveAllowed ? $locale.NUMBER_FORMATS.PATTERNS[0].posPre : "";
                var isPrefixExists = negPre == posPre == "";
                var escapedNegPre = escapeRegExp(negPre);
                var escapedPosPre = escapeRegExp(posPre);

                var escapedGroupSeparator = escapeRegExp($locale.NUMBER_FORMATS.GROUP_SEP);
                var gSize = $locale.NUMBER_FORMATS.PATTERNS[0].gSize;
                var lgSize = $locale.NUMBER_FORMATS.PATTERNS[0].lgSize;

                var decimalSeparator = $locale.NUMBER_FORMATS.DECIMAL_SEP;
                var escapedDecimalSeparator = escapeRegExp(decimalSeparator);
                var minFrac = attrs.fraction ? attrs.fraction : $locale.NUMBER_FORMATS.PATTERNS[0].minFrac;
                var maxFrac = attrs.fraction ? attrs.fraction : $locale.NUMBER_FORMATS.PATTERNS[0].maxFrac;

                var negSuf = isNegativeAllowed ? $locale.NUMBER_FORMATS.PATTERNS[0].negSuf : "";
                var posSuf = isPositiveAllowed ? $locale.NUMBER_FORMATS.PATTERNS[0].posSuf : "";
                var isSuffixExists = negSuf == posSuf == "";
                var escapedNegSuf = escapeRegExp(negSuf);
                var escapedPosSuf = escapeRegExp(posSuf);

                var regexpPrefix = (isPrefixExists ? "[" + escapedNegPre + escapedPosPre + "]?" : "");
                var regexpValue = "(\\d{1," + gSize + "}" + escapedGroupSeparator + ")?" + "(\\d{" + gSize + "}" + escapedGroupSeparator + ")*" + "(\\d{1," + lgSize + "})";
                var regexpZero = "0";
                var regexpFraction = "(" + escapedDecimalSeparator + "\\d{" + minFrac + 1 + "," + maxFrac + "}" + ")" + (minFrac === 0 ? "?" : "");
                var regexpZeroFraction = regexpFraction.replace("\\d", "0");
                var regexpSuffix = (isSuffixExists ? "[" + escapedNegSuf + escapedPosSuf + "]?" : "");

                var negativeRegExp = new RegExp("^(" + escapedNegPre + ")[^" + escapedNegPre + escapedNegSuf + "](" + escapedNegSuf + ")$");

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
                                .replace(negativeMatches && negativeMatches.length > 1 ? negPre : '', '-')
                                .replace(posPre, '')
                                .replace(new RegExp(escapedGroupSeparator, "g"), '')
                                .replace(decimalSeparator, '.')
                                .replace(negSuf, '')
                                .replace(posSuf, ''));
                            if (inRange(value, attrs.min, attrs.max)) {
                                ctrl.$setValidity('float', true);
                                return value;
                            }
                        }
                        ctrl.$setValidity('float', false);
                        return undefined;
                    });

                    ctrl.$formatters.unshift(
                        function(modelValue) {
                            return $filter('number')(parseFloat(modelValue), maxFrac);
                        }
                    );
                } else {
                    ctrl.$parsers.unshift(function (viewValue) {
                        var regexp = new RegExp(formatRegExp(regexpPrefix + regexpValue + regexpSuffix)).test(viewValue);
                        var isValid = regexp.test(viewValue);
                        if (!isValid) {
                            regexp = new RegExp(formatRegExp(regexpZero));
                            isValid = regexp.test(viewValue);
                        }
                        if (isValid) {
                            var negativeMatches = viewValue.match(negativeRegExp);
                            var value = parseFloat(viewValue
                                .replace(negativeMatches && negativeMatches.length > 1 ? negPre : '', '-')
                                .replace(posPre, '')
                                .replace(new RegExp(escapedGroupSeparator, "g"), '')
                                .replace(negSuf, '')
                                .replace(posSuf, ''));
                            if (inRange(value, attrs.min, attrs.max)) {
                                ctrl.$setValidity('integer', true);
                                return value;
                            }
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