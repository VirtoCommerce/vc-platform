(function ($) {

    //re-set all client validation given a jQuery selected form or child
    $.fn.resetValidation = function () {

        var $form = this.closest('form');

        //reset jQuery Validate's internals
        $form.validate().resetForm();

        //reset unobtrusive validation summary, if it exists
        $form.find("[data-valmsg-summary=true]")
            .removeClass("validation-summary-errors")
            .addClass("validation-summary-valid")
            .find("ul").empty();

        //reset unobtrusive field level, if it exists
        $form.find("[data-valmsg-replace]")
            .removeClass("field-validation-error")
            .addClass("field-validation-valid")
            .empty();

        return $form;
    };

    //reset a form given a jQuery selected form or a child
    //by default validation is also reset
    $.fn.formReset = function (resetValidation) {
        var $form = this.closest('form');

        $form[0].reset();

        if (resetValidation == undefined || resetValidation) {
            $form.resetValidation();
        }

        return $form;
    };

    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        var regArray = /^([^\[\]]+)\[(\d+)\]$/;

        $.each(a, function (i) {
            var name = this.name;
            var value = this.value;

            // let's also allow for "dot notation" in the input names
            var props = name.split('.');
            var position = o;
            while (props.length) {
                var key = props.shift();
                var matches;
                if (matches = regArray.exec(key)) {
                    var p = matches[1];
                    var n = matches[2];
                    if (!position[p]) position[p] = [];
                    if (!position[p][n]) position[p][n] = {};
                    position = position[p][n];
                } else {
                    if (!props.length) {
                        if (!position[key]) position[key] = value || '';
                        else if (position[key]) {
                            if (!position[key].push) position[key] = [position[key]];
                            position[key].push(value || '');
                        }
                    } else {
                        if (!position[key]) position[key] = {};
                        position = position[key];
                    }
                }
            }
        });
        return o;
    };

    $.redirect = function (url, params) {

        url = url || window.location.href || '';

        if (params != undefined)
        {
            url = url.match(/\?/) ? url : url + '?';

            for (var key in params) {
                var re = RegExp(';?' + key + '=?[^&;]*', 'g');
                url = url.replace(re, '');
                url += '&' + key + '=' + params[key];
            }
        }

        // cleanup url 
        url = url.replace(/[;&]$/, '');
        url = url.replace(/\?[;&]/, '?');
        url = url.replace(/[;&]{2}/g, '&');
        window.location.replace(url);
    };

    $.popup = function (url, title, params) {
        var urlPopup = VirtoCommerce.url(url);
        if (url.indexOf('http') == 0)
            urlPopup = url;

        var wnd = window.open(urlPopup, title ? title : 'VCF popup', params ? params : 'height=480, width=640,status=1, toolbar=1,resizable=1,scrollbars=1');
	    wnd.isPopup = true;
	    wnd.focus();
    };
    $.open = function (url)
    {
    	if (window.isPopup) {
    		$.openParent(url, true);

    	} else {
		    window.location.href = url;
	    }
    };
    $.openParent = function (url, setFocus)
    {
    	if (setFocus)
    	{
    		window.opener.focus();
    	}
    	window.opener.location.href = url;
    };

    $.showPopupMessage = function (message)
    {
        var timestamp = new Date().getTime();
        var template = '<div class="popup-alert" id="' + timestamp + '"><a title="Close" class="close">Close</a>' + message + '</div>';
        $('.popup-alert').remove();
        $('body').append(template);
        $('#' + timestamp).on('click', function () {
            closePopup(this);
        });
        window.setTimeout(function () {
            closePopup('#' + timestamp);
        }, 8000);
        
        function closePopup(popup) {
            $(popup).slideUp('slow', function ()
            {
                $(popup).remove();
            });
        }
    };
})(jQuery);