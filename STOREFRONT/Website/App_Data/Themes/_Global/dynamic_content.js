var baseUrl = window.location.host;

$(function () {
    jQuery.ajaxSettings.traditional = true;

    var placeholders = new Array("promotion-banners-1");

    $.get(baseUrl + "/DynamicContent/GetBanners", { placeholderIds: placeholders }, function (data, status) {
        if (status === "success" && data) {
            for (var i = 0; i < data.length; i++) {
                var content = "";
                for (var c = 0; c < data[i].items.length; c++) {
                    content += data[i].items[c];
                }
                $("#" + data[i].placeholder_id).html(content);
            }
        }
    });
});