/**
 * Heavily adapted from the `type="number"` directive in Angular's
 * /src/ng/directive/input.js
 */

angular.module('fiestah.money', [])
.directive('money', function () {
  'use strict';

  var NUMBER_REGEXP = /^\s*(\-|\+)?(\d+|(\d*(\.\d*)))\s*$/;

  function link(scope, el, attrs, ngModelCtrl) {
    var min, max, precision, lastValidValue, preRoundValue;

    /**
     * Returns a rounded number in the precision setup by the directive
     * @param  {Number} num Number to be rounded
     * @return {Number}     Rounded number
     */
    function round(num) {
      var d = Math.pow(10, precision);
      return Math.round(num * d) / d;
    }

    /**
     * Returns a string that represents the rounded number
     * @param  {Number} value Number to be rounded
     * @return {String}       The string representation
     */
    function formatPrecision(value) {
      return parseFloat(value).toFixed(precision);
    }

    function formatViewValue(value) {
      return ngModelCtrl.$isEmpty(value) ? '' : '' + value;
    }

    function minValidator(value) {
      if (!ngModelCtrl.$isEmpty(value) && value < min) {
        ngModelCtrl.$setValidity('min', false);
        return undefined;
      } else {
        ngModelCtrl.$setValidity('min', true);
        return value;
      }
    }

    function maxValidator(value) {
      if (!ngModelCtrl.$isEmpty(value) && value > max) {
        ngModelCtrl.$setValidity('max', false);
        return undefined;
      } else {
        ngModelCtrl.$setValidity('max', true);
        return value;
      }
    }

    ngModelCtrl.$parsers.push(function (value) {
      if (angular.isUndefined(value)) {
        value = '';
      }

      // Handle leading decimal point, like ".5"
      if (value.indexOf('.') === 0) {
        value = '0' + value;
      }

      // Allow "-" inputs only when min < 0
      if (value.indexOf('-') === 0) {
        if (min >= 0) {
          value = null;
          ngModelCtrl.$setViewValue('');
          ngModelCtrl.$render();
        } else if (value === '-') {
          value = '';
        }
      }

      var empty = ngModelCtrl.$isEmpty(value);
      if (empty || NUMBER_REGEXP.test(value)) {
        lastValidValue = (value === '')
          ? null
          : (empty ? value : parseFloat(value));
      } else {
        // Render the last valid input in the field
        ngModelCtrl.$setViewValue(formatViewValue(lastValidValue));
        ngModelCtrl.$render();
      }

      ngModelCtrl.$setValidity('number', true);

      return lastValidValue;
    });
    ngModelCtrl.$formatters.push(formatViewValue);


    // Min validation
    attrs.$observe('min', function (value) {
      min = parseFloat(value || 0);
      minValidator(ngModelCtrl.$modelValue);
    });

    ngModelCtrl.$parsers.push(minValidator);
    ngModelCtrl.$formatters.push(minValidator);


    // Max validation (optional)
    if (angular.isDefined(attrs.max)) {
      attrs.$observe('max', function (val) {
        max = parseFloat(val);
        maxValidator(ngModelCtrl.$modelValue);
      });

      ngModelCtrl.$parsers.push(maxValidator);
      ngModelCtrl.$formatters.push(maxValidator);
    }


    // Round off (disabled by "-1")
    if (attrs.precision !== '-1') {
      attrs.$observe('precision', function (value) {
        var parsed = parseFloat(value);
        precision = !isNaN(parsed) ? parsed : 2;

        // Trigger $parsers and $formatters pipelines
        ngModelCtrl.$setViewValue(formatPrecision(ngModelCtrl.$modelValue));
      });

      ngModelCtrl.$parsers.push(function (value) {
        if (value) {
          // Save with rounded value
          lastValidValue = round(value);

          return lastValidValue;
        } else {
          return undefined;
        }
      });
      ngModelCtrl.$formatters.push(function (value) {
        return value ? formatPrecision(value) : value;
      });

      // Auto-format precision on blur
      el.bind('blur', function () {
        var value = ngModelCtrl.$modelValue;
        if (value) {
          ngModelCtrl.$viewValue = formatPrecision(value);
          ngModelCtrl.$render();
        }
      });
    }
  }

  return {
    restrict: 'A',
    require: 'ngModel',
    link: link
  };
});
