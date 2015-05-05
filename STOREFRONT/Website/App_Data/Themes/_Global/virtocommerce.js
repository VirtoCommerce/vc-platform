$(function () {
    VirtoCommerce = function () {
        this.renderDynamicContent();
    }

    VirtoCommerce.prototype.constructor = VirtoCommerce.prototype;

    VirtoCommerce.prototype = {
        initialize: function () {
        },
        url: function (url) {
            var baseUrl = window.location.protocol + "//" + location.host;
            return baseUrl + url;
        },
        renderDynamicContent: function () {
            var url = this.url("/banner/ShowDynamicContents");
            var placeholders = $("[data-vccontentid]");
            var placeholderIds = new Array();
            if (placeholders.length) {
                url += "?";
                for (var i = 0; i < placeholders.length; i++) {
                    var placeholderId = $(placeholders[i]).data("vccontentid");
                    placeholderIds.push(placeholderId);
                    url += "placename=" + placeholderId;
                    if (i < placeholders.length - 1) {
                        url += "&";
                    }
                }
            }
            $.get(url, function (htmlResponse) {
                if (htmlResponse) {
                    var htmlData = $("<div />").html(htmlResponse);
                    for (var i = 0; i < placeholderIds.length; i++) {
                        var htmlPlaceContent = htmlData.find("#" + placeholderIds[i]).html();
                        $("[data-vccontentid='" + placeholderIds[i] + "']").html(htmlPlaceContent);
                    }
                }
            });
        }
    }

    virtoCommerce = new VirtoCommerce();
});