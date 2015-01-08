/// <reference path="jquery-1.4.4-vsdoc.js" />
/// <reference path="jquery.validate.unobtrusive.js" />

$.validator.addMethod('requiredif',
    function (value, element, parameters) {
        var id = '#' + parameters['dependentproperty'];

        // get the target value (as a string, 
        // as that's what actual value will be)
        var targetvalue = parameters['targetvalue'];
        targetvalue = 
          (targetvalue == null ? '' : targetvalue).toString();

        // get the actual value of the target control
        // note - this probably needs to cater for more 
        // control types, e.g. radios
        var control = $(id);
        var controltype = control.attr('type');
        var actualvalue =
            controltype === 'checkbox' ?
            control.is(':checked').toString() :
            control.val();

        // if the condition is true, reuse the existing 
        // required field validator functionality
        if (targetvalue === actualvalue)
            return $.validator.methods.required.call(
              this, value, element, parameters);

        return true;
    }
);

$.validator.unobtrusive.adapters.add(
    'requiredif', ['dependentproperty', 'targetvalue'], 
    function (options) {
        options.rules['requiredif'] = {
            dependentproperty: options.params['dependentproperty'],
            targetvalue: options.params['targetvalue']
        };
        options.messages['requiredif'] = options.message;
    });

$.validator.unobtrusive.adapters.add('dynamicrange', ['minvalueproperty', 'maxvalueproperty'],
       function (options) {
           options.rules['dynamicrange'] = options.params;
           if (options.message != null) {
               $.validator.messages.dynamicrange = options.message;
           }
       }
   );

$.validator.addMethod('dynamicrange', function (value, element, params) {
    var minValue = parseInt($('input[name="' + params.minvalueproperty + '"]').val(), 10);
    var maxValue = parseInt($('input[name="' + params.maxvalueproperty + '"]').val(), 10);
    var currentValue = parseInt(value, 10);
    if (isNaN(minValue) || isNaN(maxValue) || isNaN(currentValue) || minValue > currentValue || currentValue > maxValue) {
        var message = $(element).attr('data-val-dynamicrange');
        $.validator.messages.dynamicrange = $.format(message, minValue, maxValue);
        return false;
    }
    return true;
}, '');

jQuery.validator.unobtrusive.adapters.add("brequired", function (options) {
    //b-required for checkboxes
    if (options.element.tagName.toUpperCase() == "INPUT" && options.element.type.toUpperCase() == "CHECKBOX") {
        //setValidationValues(options, "required", true);
        options.rules["required"] = true;
        if (options.message) {
            options.messages["required"] = options.message;
        }
    }
});

//Override jquery to allow comma in decimal numbers more: http://rebuildall.umbraworks.net/2011/03/02/jQuery_validate_and_the_comma_decimal_separator 
$.validator.methods.range = function(value, element, param) {
    var globalizedValue = value.replace(",", ".");
    return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
};

$.validator.methods.number = function(value, element) {
    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
};
