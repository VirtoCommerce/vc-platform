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

        $.get(url, function(htmlResponse) {
            if (htmlResponse) {
                var htmlData = $("<div />").html(htmlResponse);
                for (var i = 0; i < placeholderIds.length; i++) {
                    var htmlPlaceContent = htmlData.find("#" + placeholderIds[i]).html();
                    $("[data-vccontentid='" + placeholderIds[i] + "']").html(htmlPlaceContent);
                }

                $(".flexslider").flexslider();
            }
        });
    }
};

$(function () {
    VirtoCommerce.renderDynamicContent();

    $("#addToQuote").on("click", function () {
        $.ajax({
            type: "POST",
            url: VirtoCommerce.url("/quote/add"),
            data: {
                variantId: $("#productSelect").val()
            },
            success: function (jsonResult) {
                if (jsonResult) {
                    var quoteCount = $("#quoteCount");
                    quoteCount.text(jsonResult.items_count);
                    if (jsonResult.items_count > 0) {
                        quoteCount.removeClass("hidden-count");
                    } else {
                        quoteCount.addClass("hidden-count");
                    }
                }
            }
        });
    });

    $("body").delegate(".cart-row.quote .js-qty .js--add", "click", function () {
        var id = $(this).data("id");
        var quantityInput = $(this).parents(".js-qty").find("input");
        var quantity = parseInt(quantityInput.val()) + 1;
        quantityInput.val(quantity);
    });

    $("body").delegate(".cart-row.quote .js-qty .js--minus", "click", function () {
        var id = $(this).data("id");
        var quantityInput = $(this).parents(".js-qty").find("input");
        var quantity = parseInt(quantityInput.val()) - 1;
        if (quantity >= 1) {
            quantityInput.val(quantity);
        }
    });

    $(".add-tier").on("click", function (event) {
        event.preventDefault();

        var tierHtml = "<div class=\"js-qty\">";
        tierHtml += "<div class=\"js-qty--inner\">";
        tierHtml += "<input class=\"js--num\" pattern=\"[0-9]*\" type=\"text\" value=\"1\" />";
        tierHtml += "<span class=\"js--qty-adjuster js--add\">+</span>";
        tierHtml += "<span class=\"js--qty-adjuster js--minus\">-</span>";
        tierHtml += "</div>";
        tierHtml += "<a class=\"link-action\">Remove</a>";
        tierHtml += "</div>";

        var qtyCount = $(this).parents(".grid-item").find(".js-qty").length - 1;
        var predLastQty = $(this).parents(".grid-item").find(".js-qty:eq(" + (qtyCount - 1) + ")");

        predLastQty.after(tierHtml);
    });

    $("body").delegate(".js-qty .link-action", "click", function () {
        $(this).parents(".js-qty").remove();
    });

    $(".ublock button").on("click", function () {
        var quoteRequest = {
            Id: $("#quote_request_id").val(),
            Comment: $("#quote_request_comment").val(),
            Email: $("#quote_request_email").val(),
            FirstName: $("#quote_request_first_name").val(),
            LastName: $("#quote_request_last_name").val(),
            Items: []
        };
        $.each($(".cart-row.quote"), function () {
            var itemElement = $(this);
            var quoteItem = {
                Id: itemElement.data("id"),
                Comment: itemElement.find(".quote_item_comment").val(),
                ProposalPrices: []
            };

            $.each(itemElement.find(".js--num"), function () {
                var tierPrice = {
                    Quantity: parseInt($(this).val()),
                    Price: 0
                };

                quoteItem.ProposalPrices.push(tierPrice);
            });

            quoteRequest.Items.push(quoteItem);
        });

        $.ajax({
            type: "POST",
            url: VirtoCommerce.url("/quote/submit"),
            data: quoteRequest,
            success: function (jsonResult) {
                window.location.href = jsonResult.redirect_url;
            }
        });
    });

    $("#btn-checkout-quote-request").on("click", function () {
        var quoteRequest = getQuoteRequest();
        $.ajax({
            type: "POST",
            url: VirtoCommerce.url("/account/quote/checkout"),
            data: quoteRequest,
            success: function (jsonResponse) {
                if (jsonResponse) {
                    window.location.href = jsonResponse.redirect_url;
                }
            }
        });
    });

    $(".proposal-price-radio").on("change", function () {
        var quoteRequest = getQuoteRequest();
        recalculateQuoteRequestTotals(quoteRequest);
    });
});

var getQuoteRequest = function () {
    var quoteRequest = {
        Number: $("#quote-request-number").data("number"),
        Items: []
    };
    $.each($(".cart-row.quote"), function () {
        var selectedProposalPrice = $(this).find(".proposal-price-radio:checked");
        var quoteItem = {
            Id: $(this).data("id"),
            SelectedTierPrice: {
                Quantity: selectedProposalPrice.data("quantity"),
                Price: selectedProposalPrice.val()
            }
        };
        quoteRequest.Items.push(quoteItem);
    });
    return quoteRequest;
}

var recalculateQuoteRequestTotals = function (quoteRequest) {
    $.ajax({
        type: "POST",
        url: VirtoCommerce.url("/quote/recalculate"),
        data: quoteRequest,
        success: function (jsonResponse) {
            $("#quote-subtotal").text(jsonResponse.totals.sub_total_exl_tax);
            $("#quote-tax-total").text(jsonResponse.totals.tax_total);
            $("#quote-grand-total").text(jsonResponse.totals.grand_total_incl_tax);
        }
    });
}