$(function () {
    $("header .row.nav-desktop .columns.large-3").on("mouseover", function () {
        $(this).find("ul.row.nav-sub").show();
    });
    $("header .row.nav-desktop .columns.large-3").on("mouseleave", function () {
        $(this).find("ul.row.nav-sub").hide();
    });
    $("select[id^='selProductOptions']").on("change", function () {
        window.location.href = replaceUrlParam(window.location.href, "variant", $(this).val());
    });
    $("#btn-add-to-cart").on("click", function () {
        var btnElement = $(this);
        var variantId = btnElement.data("id");
        if (!btnElement.is(":disabled")) {
            $.ajax({
                type: "POST",
                url: VirtoCommerce.url("/cart/add?id=" + variantId),
                success: function () {
                    window.location.href = VirtoCommerce.url("/cart");
                }
            });
        }
    });
});

function replaceUrlParam(url, paramName, paramValue) {
    var pattern = new RegExp('\\b(' + paramName + '=).*?(&|$)')
    if (url.search(pattern) >= 0) {
        return url.replace(pattern, '$1' + paramValue + '$2');
    }
    return url + (url.indexOf('?') > 0 ? '&' : '?') + paramName + '=' + paramValue
}