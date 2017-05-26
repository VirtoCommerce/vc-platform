angular.module('platformWebApp')
.factory('platformWebApp.numberUtils', ['$filter', '$locale', function ($filter, $locale) {
        // Service provide variables and functions for number (float & integer) convertion and validation
        var result = {
            escape: function (str) {
                return RegExp.escape(str).replace(/\s/g, " ");
            },
            formatRegExp: function(str) {
                return "^" + str + "$";
            },
            getVariables: function(min, max, fraction) {
                var result = {};

                result["isNegativeAllowed"] = !(min && min >= 0);
                result["isPositiveAllowed"] = !(max && max <= 0);

                result["negativePrefix"] = $locale.NUMBER_FORMATS.PATTERNS[0].negPre;
                result["positivePrefix"] = $locale.NUMBER_FORMATS.PATTERNS[0].posPre;
                result["isPrefixExists"] = result.negativePrefix == result.positivePrefix == "";
                result["escapedNegativePrefix"] = this.escape(result.negativePrefix);
                result["escapedPositivePrefix"] = this.escape(result.positivePrefix);

                result["escapedGroupSeparator"] = this.escape($locale.NUMBER_FORMATS.GROUP_SEP);
                result["groupSize"] = $locale.NUMBER_FORMATS.PATTERNS[0].gSize;
                result["lastGroupSize"] = $locale.NUMBER_FORMATS.PATTERNS[0].lgSize;

                result["decimalSeparator"] = $locale.NUMBER_FORMATS.DECIMAL_SEP;
                result["escapedDecimalSeparator"] = this.escape(result.decimalSeparator);
                result["minimalFraction"] = fraction ? fraction : $locale.NUMBER_FORMATS.PATTERNS[0].minFrac;
                result["maximumFraction"] = fraction ? fraction : $locale.NUMBER_FORMATS.PATTERNS[0].maxFrac;

                result["negativeSuffix"] = $locale.NUMBER_FORMATS.PATTERNS[0].negSuf;
                result["positiveSuffix"] = $locale.NUMBER_FORMATS.PATTERNS[0].posSuf;
                result["isSuffixExists"] = result.negativeSuffix == result.positiveSuffix == "";
                result["escapedNegativeSuffix"] = this.escape(result.negativeSuffix);
                result["escapedPositiveSuffix"] = this.escape(result.positiveSuffix);

                result["regexpPrefix"] = (result.isPrefixExists ? "[" + (result.isNegativeAllowed ? result.escapedNegativePrefix : "") + (result.isPositiveAllowed ? result.escapedPositivePrefix : "") + "]?" : "");
                result["regexpValue"] = "(((\\d{1," + result.groupSize + "}" + result.escapedGroupSeparator + ")?" + "(\\d{" + result.groupSize + "}" + result.escapedGroupSeparator + ")*" + "(\\d{1," + result.lastGroupSize + "}))|\\d*)";
                result["regexpZero"] = "0";
                result["regexpFraction"] = "(" + result.escapedDecimalSeparator + "\\d{" + result.minimalFraction + 1 + "," + result.maximumFraction + "}" + ")" + (result.minimalFraction === 0 ? "?" : "");
                result["regexpZeroFraction"] = result.regexpFraction.replace("\\d", "0");
                result["regexpSuffix"] = (result.isSuffixExists ? "[" + (result.isNegativeAllowed ? result.escapedNegativeSuffix : "") + (result.isPositiveAllowed ? result.escapedPositiveSuffix : "") + "]?" : "");

                result["negativeRegExp"] = new RegExp("^(" + result.escapedNegativePrefix + ")[^" + result.escapedNegativePrefix + result.escapedNegativeSuffix + "](" + result.escapedNegativeSuffix + ")$");

                return result;
            },
            inRange: function(value, min, max, setValidity)
            {
                var isGreaterThanOrEqualToMin = true;
                var isLessThanOrEqualToMax = true;
                if (min) {
                    isGreaterThanOrEqualToMin = min <= value;
                    if (setValidity)
                        setValidity('min', isGreaterThanOrEqualToMin);
                }
                if (max) {
                    isLessThanOrEqualToMax = value <= max;
                    if (setValidity)
                        setValidity('max', isLessThanOrEqualToMax);
                }
                if (!isGreaterThanOrEqualToMin || !isLessThanOrEqualToMax) {
                    return undefined;
                }
                return value;
            },
            validate: function (viewValue, numType, min, max, fraction, setValidity) {
                var vars;
                var regexp;
                var isValid;
                var negativeMatches;
                var value;
                if (numType === "float") {
                    vars = this.getVariables(min, max, fraction);
                    regexp = new RegExp(this.formatRegExp(vars.regexpPrefix + vars.regexpValue + vars.regexpFraction + vars.regexpSuffix));
                    isValid = regexp.test(viewValue);
                    if (!isValid) {
                        regexp = new RegExp(this.formatRegExp(vars.regexpZero + vars.regexpZeroFraction));
                        isValid = regexp.test(viewValue);
                    }
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
                        return this.inRange(value, min, max, setValidity);
                    }
                    if (setValidity)
                        setValidity('float', false);
                    return undefined;
                } else {
                    vars = this.getVariables();
                    regexp = new RegExp(this.formatRegExp(vars.regexpPrefix + vars.regexpValue + vars.regexpSuffix));
                    isValid = regexp.test(viewValue);
                    if (!isValid) {
                        regexp = new RegExp(this.formatRegExp(vars.regexpZero));
                        isValid = regexp.test(viewValue);
                    }
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
                        return this.inRange(value, min, max, setValidity);
                    }
                    if (setValidity)
                        setValidity('integer', false);
                    return undefined;
                }
            },
            format: function (modelValue, numType, min, max, fraction) {
                if (numType === "float") {
                    return $filter('number')(parseFloat(modelValue), this.getVariables(min, max, fraction).maximumFraction);
                } else {
                    return $filter('number')(parseFloat(modelValue), 0);
                }
            }
        };
        return result;
    }
]);