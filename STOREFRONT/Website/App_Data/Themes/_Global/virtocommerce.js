if ((typeof VirtoCommerce) == "undefined") {
    var VirtoCommerce = {};
}

Array.prototype.getElementByVal = function (propName, propValue) {
    var el = null;
    for (var i = 0; i < this.length; i++) {
        if (this[i][propName] == propValue) {
            el = this[i];
            break;
        }
    }
    return el;
}

VirtoCommerce.changeCurrency = function (currencyCode) {
    VirtoCommerce.redirect(location.href, { currency: currencyCode });
};

VirtoCommerce.redirect = function(url, params) {

    url = url || window.location.href || '';

    if (params != undefined) {
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

VirtoCommerce.url = function (url) {
    //var baseUrl = window.location.protocol + "//" + location.host;
    var baseUrl = $("base").attr("href");
    return baseUrl + url;
};

VirtoCommerce.renderDynamicContent = function () {
    var url = VirtoCommerce.url("/banners");
    var placeholders = $("[data-vccontentid]");
    var placeholderIds = new Array();
    if (placeholders.length) {
        url += "?";
        for (var i = 0; i < placeholders.length; i++) {
            var placeholderId = $(placeholders[i]).data("vccontentid");
            placeholderIds.push(placeholderId);
            url += "placenames=" + placeholderId;
            if (i < placeholders.length - 1) {
                url += "&";
            }
        }

        $.get(url, function (jsonResponse) {
            if (jsonResponse) {
                for (var i = 0; i < jsonResponse.length; i++) {
                    var placeholder = jsonResponse[i];
                    var placeholderElement = $("[data-vccontentid='" + placeholder.name + "']");
                    if (placeholderElement.length) {
                        placeholderElement.empty();
                        for (var j = 0; j < placeholder.banners.length; j++) {
                            var banner = placeholder.banners[j];
                            if (banner.content_type.toLowerCase() == "slider") {
                                placeholderElement.append("<div class=\"flexslider\"><ul class=\"slides\"></ul></div>");
                                for (var k = 0; k < banner.properties.image_links.length; k++) {
                                    var image = banner.properties.image_links[k].value;
                                    placeholderElement.find(".slides").append("<li><img src=\"" + image + "\" /></li>");
                                }
                            } else {
                                // Another dynamic content type
                            }
                        }
                    }
                }
            }

            $(".flexslider").flexslider();
        });

        //$.get(url, function(htmlResponse) {
            //if (htmlResponse) {
                //var htmlData = $("<div />").html(htmlResponse);
                //for (var i = 0; i < placeholderIds.length; i++) {
                //    var htmlPlaceContent = htmlResponse.find("#" + placeholderIds[i]).html();
                //    $("[data-vccontentid='" + placeholderIds[i] + "']").html(htmlPlaceContent);
                //}
            //}
        //});
    }
};

$(function () {
    VirtoCommerce.renderDynamicContent();
});