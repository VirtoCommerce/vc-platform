var classes = { groupIdentifier: ".form-group", error: "has-error", success: null };

function updateClasses(inputElement, toAdd, toRemove) {
    // For checkout
    var group = inputElement.closest(classes.groupIdentifier);
    if (group.length) {
        group.addClass(toAdd).removeClass(toRemove);
    } else {
        // For other forms
        inputElement.addClass(toAdd).removeClass(toRemove);
    }
}

function onError(inputElement, message) {
    if (window.location.href.indexOf("checkout") >= 0) {
        updateClasses(inputElement, classes.error, classes.success);
        if (inputElement.prop("id").indexOf("shipping_method") >= 0) {
            $("#modal-error .modal-body").html("Shipping method was not setted.");
            $("#modal-error").modal("show");
        } else if (inputElement.prop("id").indexOf("payment_method") >= 0) {
            $("#modal-error .modal-body").html("Payment method was not setted.");
            $("#modal-error").modal("show");
        } else {
            var options = {
                content: message,
                container: "body",
                html: true,
                placement: "bottom",
                trigger: "focus"
            };
            inputElement.popover("destroy")
                .addClass("error")
                .popover(options);
            $(".popover").css("overflow", "visible");
        }
    } else {
        updateClasses(inputElement, "error", "");
        inputElement.next("span.text-error").remove();
        $("<span class=\"text-error\">" + message + "</span>").insertAfter(inputElement);
    }
}

function onSuccess(inputElement) {
    if (window.location.href.indexOf("checkout") >= 0) {
        updateClasses(inputElement, classes.success, classes.error);
        inputElement.popover("destroy");
    } else {
        updateClasses(inputElement, "", "error");
        inputElement.next("span.text-error").remove();
    }
}

function onValidated(errorMap, errorList) {
    $.each(errorList, function () {
        onError($(this.element), this.message);
    });
    if (this.settings.success) {
        $.each(this.successList, function () {
            onSuccess($(this));
        });
    }
}

$(function () {
    $("form").each(function () {
        var validator = $(this).data("validator");
        validator.settings.showErrors = onValidated;
        validator.settings.ignore = "";
    });
});